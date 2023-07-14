using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Text.Json;
using System.Net.Http;
using System.Linq;
using System.IO;
using System;

using xabbo_music.Extensions;
using Xabbo.Core.GameData;
using xabbo_music.Enum;
using Xabbo.Core;

namespace xabbo_music.GameStateManagers
{
    public static class FurnidataManager
    {
        public static string Path = "files/furnidata/";
        public static string TextsPath = "files/external_texts/";
        public static FurniData Furni { get; set; }

        public static async Task<bool> LoadAsync(HHotel hotel, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!Directory.Exists("files/")) Directory.CreateDirectory("files/");
                if (!Directory.Exists(Path)) Directory.CreateDirectory(Path);
                if (!Directory.Exists(TextsPath)) Directory.CreateDirectory(TextsPath);

                if (File.Exists($"{Path}{hotel}.txt"))
                {
                    Furni = FurniData.LoadJson(File.ReadAllText($"{Path}{hotel}.txt"));
                    return true;
                }

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.131 Safari/537.36");

                GameDataHashesContainer? hashesContainer = null;

                string json = await httpClient.GetStringAsync($"{hotel.HotelToUrl()}gamedata/hashes2", cancellationToken);
                hashesContainer = JsonSerializer.Deserialize<GameDataHashesContainer>(json);

                if (hashesContainer is null)
                    throw new Exception("Failed to deserialize game data hashes.");

                foreach (var entry in hashesContainer.Hashes)
                {
                    if (entry.Name == "furnidata")
                        await LoadDataAsync(Path, httpClient, entry.Url, entry.Hash, hotel, cancellationToken);

                    else if (entry.Name == "external_texts")
                        await LoadDataAsync(TextsPath, httpClient, entry.Url, entry.Hash, hotel, cancellationToken);
                }

                if (Furni != null)
                    return true;
            }

            catch (Exception ex)
            {
                Furni = null;
            }

            return false;
        }

        private static async Task LoadDataAsync(string path, HttpClient http, string url, string hash, HHotel hotel, CancellationToken cancellationToken)
        {
            var response = await http.GetAsync($"{url}/{hash}", cancellationToken);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            File.WriteAllText($"{path}{hotel}.txt", json);

            if (Path == path)
                Furni = FurniData.LoadJson(json);
        }

        private static ConcurrentDictionary<(int, ItemType), FurniInfo> CachedKindType { get; set; } = new();
        private static ConcurrentDictionary<string, FurniInfo> CachedIdentifier { get; set; } = new();

        public static FurniInfo? GetFurniInfo(int kind, ItemType itemType)
        {
            if (CachedKindType.TryGetValue((kind, itemType), out FurniInfo cachedData))
                return cachedData;

            var furniInfo = Furni.FirstOrDefault(x => x.Kind == kind && x.Type == itemType);

            if (furniInfo == null)
                return null;

            try
            {
                CachedIdentifier.TryAdd(furniInfo.Identifier, furniInfo);
                CachedKindType.TryAdd((kind, itemType), furniInfo);
            }

            catch { }

            return furniInfo;
        }

        public static FurniInfo GetFurniInfo(string identifier)
        {
            if (CachedIdentifier.TryGetValue(identifier, out FurniInfo cachedData))
                return cachedData;

            var furniInfo = Furni.FirstOrDefault(x => x.Identifier == identifier);
            if (furniInfo == null) return furniInfo;

            CachedKindType.TryAdd((furniInfo.Kind, furniInfo.Type), furniInfo);
            CachedIdentifier.TryAdd(furniInfo.Identifier, furniInfo);

            return furniInfo;
        }

        public static IEnumerable<FurniInfo> FindItems(string searchText)
        {
            string searchText2 = searchText;
            searchText2 = searchText2.ToLower();

            return Furni.Where(x => x.Name.Replace(" ", "").ToLower().Contains(searchText2) && (x.Type == ItemType.Floor || x.Type == ItemType.Wall) && !x.IsBuildersClub && !x.Identifier.StartsWith("noob_") && !x.Identifier.Contains("_nt_") && !x.Name.StartsWith("wl_") && !x.Name.StartsWith("lido") && !x.Identifier.StartsWith("test_") && !x.Name.Equals("Habbo Hotel") && !x.Name.StartsWith("habbo15_win") && !x.Identifier.StartsWith("room_noob") && !x.Identifier.StartsWith("present_") && !x.Name.StartsWith("theatre_") && !x.Name.StartsWith("pcnc_") && !x.Identifier.StartsWith("nft_"));
        }
    }
}
