using xabbo_music.GameStateManagers;
using xabbo_music.Extensions;
using xabbo_music.Misc;
using xabbo_music.Enum;

using Point = System.Drawing.Point;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Linq;

using Xabbo.Core.Extensions;
using Xabbo.Core.GameData;
using Xabbo.Core.Events;
using Xabbo.Core.Tasks;
using Xabbo.Core.Game;
using Xabbo.Extension;
using Xabbo.GEarth;
using Xabbo.Core;

using GetCatalogPageTask = xabbo_music.Game.GetCatalogPageTask;

namespace xabbo_music
{
    public class MusicExtension : GEarthExtension
    {
        private Dictionary<string, ICatalogPage> bcCatalogPages = new();
        private Dictionary<string, Point> effectMap = new();
        private Dictionary<string, Note> allNotes = new();

        public bool RoomLoaded => RoomHeightmap != null;
        public IFloorPlan? RoomHeightmap;
        private RoomManager roomManager;
        private int currentPlaceFurniId;

        public MusicExtension(GEarthOptions options)
            : base(options)
        { 
            roomManager = new RoomManager(this);
            roomManager.Entered += OnRoomEntered;
        }

        protected override void OnConnected(GameConnectedEventArgs e)
        {
            base.OnConnected(e);

            if (Client != Xabbo.ClientType.Flash)
            {
                MessageBox.Show("Sorry! This extension is only supported on the Flash version :/");
                MainWindow.Instance.Close();
            }

            _ = Task.Run(() => InitializeXabboCoreExtensions(e.Host.HostToHotel()));
        }

        private async Task InitializeXabboCoreExtensions(HHotel hotel)
        {
            await FurnidataManager.LoadAsync(hotel);

            if (FurnidataManager.Furni == null)
            {
                MessageBox.Show("Couldn't load furnidata, please restart the extension");
                return;
            }

            var text = ExternalTexts.Load($"files/external_texts/{hotel}.txt");

            XabboCoreExtensions.Initialize(FurnidataManager.Furni, text);
        }

        #region Methods to fetch catalog
        ICatalog bcCatalogRoot { get; set; }
        ICatalogPage GetBcPageNamed(string name) => GetBcCatalogPage(bcCatalogRoot.First(x => x.Name == name).Id);
        public ICatalog GetCatalog(string type = "NORMAL", int timeout = 5000) => new GetCatalogTask(this, type).Execute(timeout, new());
        public ICatalog GetBcCatalog(int timeout = 5000) => GetCatalog("BUILDERS_CLUB", timeout);
        public ICatalogPage GetCatalogPage(int pageId, string type = "NORMAL", int timeout = 5000) => new GetCatalogPageTask(this, pageId, type).Execute(timeout, new());
        public ICatalogPage GetBcCatalogPage(int pageId, int timeout = 5000) => GetCatalogPage(pageId, "BUILDERS_CLUB", timeout);

        Dictionary<string, ICatalogPage> GetCatalogPages()
        {
            bcCatalogRoot = GetBcCatalog();

            var result = new Dictionary<string, ICatalogPage>()
            {
              { "gaming_furni", GetBcPageNamed("gaming_furni") },
              { "wf_cnd", GetBcPageNamed("wired_conditions") },
              { "wf_trg", GetBcPageNamed("wired_triggers") },
              { "wf_act", GetBcPageNamed("wired_effects") },
              { "wf_add", GetBcPageNamed("wired_addons") },
              { "sfx", GetBcPageNamed("sound_fx") },
            };

            return result;
        }
        Dictionary<string, ICatalogOffer> GetCatalogOffers()
        {
            var result = new Dictionary<string, ICatalogOffer>()
            {
              { "wf_cnd_match_snapshot", bcCatalogPages["wf_cnd"].Offers.First(x => x.Products.Any(p => p.GetIdentifier() == "wf_cnd_match_snapshot")) },
              { "wf_act_match_to_sshot", bcCatalogPages["wf_act"].Offers.First(x => x.Products.Any(p => p.GetIdentifier() == "wf_act_match_to_sshot")) },
              { "wf_trg_at_given_time", bcCatalogPages["wf_trg"].Offers.First(x => x.Products.Any(p => p.GetIdentifier() == "wf_trg_at_given_time")) },
              { "wf_cnd_has_altitude", bcCatalogPages["wf_cnd"].Offers.First(x => x.Products.Any(p => p.GetIdentifier() == "wf_cnd_has_altitude")) },
              { "wf_trg_period_short", bcCatalogPages["wf_trg"].Offers.First(x => x.Products.Any(p => p.GetIdentifier() == "wf_trg_period_short")) },
              { "wf_act_set_altitude", bcCatalogPages["wf_act"].Offers.First(x => x.Products.Any(p => p.GetIdentifier() == "wf_act_set_altitude")) },
              { "wf_act_toggle_state", bcCatalogPages["wf_act"].Offers.First(x => x.Products.Any(p => p.GetIdentifier() == "wf_act_toggle_state")) },
              { "wf_xtra_or_eval", bcCatalogPages["wf_add"].Offers.First(x => x.Products.Any(p => p.GetIdentifier() == "wf_xtra_or_eval")) },
              { "wf_glowball", bcCatalogPages["gaming_furni"].Offers.First(x => x.Products.Any(p => p.GetIdentifier() == "wf_glowball")) },
            };

            foreach (var note in allNotes.Values)
            {
                if (!result.ContainsKey(note.Furni))
                {
                    result.Add(note.Furni, bcCatalogPages["sfx"].Offers.First(x => x.Products.Any(p => p.GetIdentifier() == note.Furni)));
                }
            }

            return result;
        }
        #endregion

        private void OnRoomEntered(object sender, RoomEventArgs e)
        {
            effectMap.Clear();
            RoomHeightmap = e.Room.FloorPlan;

            if (MainWindow.TileSelectorWindow != null && MainWindow.TileSelectorWindow.IsVisible)
            {
                MainWindow.TileSelectorWindow.SetItemAmounts();
                MainWindow.TileSelectorWindow.HabboTileMap.RenderRoom(RoomHeightmap.OriginalString.Split("\r").ToList());
            }
        }

        Dictionary<string, IEnumerable<IFloorItem>> GetRoomEffects()
        {
            var result = new Dictionary<string, IEnumerable<IFloorItem>>();

            foreach (var note in allNotes.Values)
            {
                if (!result.ContainsKey(note.Furni))
                {
                    result.Add(note.Furni, roomManager.Room.FloorItems.Where(x => x.GetIdentifier() == note.Furni));
                }
            }

            return result;
        }

        Point GetSFXLocation(string sfxFurniIdentifier, Point effectSpace)
        {
            if (!effectMap.ContainsKey(sfxFurniIdentifier))
            {
                effectMap.Add(sfxFurniIdentifier, effectSpace);
            }

            return effectMap[sfxFurniIdentifier];
        }

        Dictionary<string, Note> GetAllNotes()
        {
            var result = new Dictionary<string, Note>();
            var height = "0";

            foreach (var noteArray in Effects.IdentifiersAndNotes)
            {
                foreach (var note in noteArray.Item2)
                {
                    result.Add(note, new Note() { Height = height, Furni = noteArray.Item1 });
                    height = (int.Parse(height) + 50).ToString();
                }

                height = "0";
            }

            return result;
        }

        public List<string> GetCurrentSoundNotes(int offset = 0)
        {
            var allNotesInSong = new List<string>();

            foreach (var subpart in MainWindow.FullSong)
            {
                if (subpart.Item1 < offset)
                    continue;

                var stringList = new List<string>();

                foreach (var chr in subpart.Item3.ToCharArray())
                    stringList.Add(chr.ToString());

                allNotesInSong.AddRange(stringList);
            }

            return allNotesInSong;
        }

        public async Task PlaySong(CancellationTokenSource cancellationToken)
        {
            var offset = MainWindow.Instance.Timeline.Timelines[0][MainWindow.Instance.Timeline.CurrentSelectorIndex].CurrentDelay;

            var notes = new List<(Note, (int, int, string))>();
            var allNotesInSong = GetCurrentSoundNotes(offset);
            allNotes = GetAllNotes();

            var placedSfx = new List<(string, string, string, string)>();

            foreach (var note in MainWindow.FullSong)
            {
                if (note.Item1 < offset)
                    continue;

                var noteList = new List<(Note, (int, int, string))>();

                foreach (var subNote in note.Item3.ToCharArray())
                {
                    if (allNotes.ContainsKey(subNote.ToString()))
                    {
                        var _note = allNotes[subNote.ToString()];
                        noteList.Add((_note, note));
                    }
                }

                notes.AddRange(noteList);
            }

            foreach (var note in allNotes)
            {
                if (!allNotesInSong.Contains(note.Key)) continue;

                await SendAsync(In["ObjectAdd"], currentPlaceFurniId, FurnidataManager.Furni.First(x => x.Identifier == note.Value.Furni).Kind, 999, 999, 0, $"{(int.Parse(note.Value.Height) / 100.0).ToString().Replace(",", ".")}", "0.0", 0, 0, "0", -1, 1, 1, "xabbo-music");
                await Task.Delay(1);
                placedSfx.Add((note.Value.Furni, $"{currentPlaceFurniId}", "0", note.Value.Height));
                currentPlaceFurniId++;
            }

            var groupedNotes = notes.GroupBy(x => x.Item2.Item1).ToList();
            var orderedNotes = groupedNotes.OrderBy(x => x.Key).ToList();

            await Task.Delay(500);

            for (var i = 0; i < orderedNotes.Count; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                    continue;

                foreach (var note in orderedNotes[i].ToList())
                {
                    var notee = placedSfx.FirstOrDefault(x => x.Item1 == note.Item1.Furni && x.Item4 == note.Item1.Height);
                    await PlayNote(notee);
                }

                var delay = i + 1 < orderedNotes.Count ? orderedNotes[i + 1].Key - orderedNotes[i].Key : 0;

                while (delay > 1000)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    await Task.Delay(1000);
                    delay -= 1000;
                }

                if (!cancellationToken.IsCancellationRequested)
                    await Task.Delay(delay);
            }

            if (!cancellationToken.IsCancellationRequested)
                await Task.Delay(1000);

            for (var i = 0; i < placedSfx.Count; i++)
            {
                await SendAsync(In["ObjectRemove"], placedSfx[i].Item2, false, 1, 0);
            }

            MainWindow.Instance.PlayButton.Visibility = Visibility.Visible;
            MainWindow.Instance.StopButton.Visibility = Visibility.Hidden;

            async Task PlayNote((string, string, string, string) furni)
            {
                var index = placedSfx.IndexOf(furni);
                placedSfx[index] = (furni.Item1, furni.Item2, furni.Item3 == "0" ? "1" : "0", furni.Item4);

                await SendAsync(In["ObjectDataUpdate"], furni.Item2, 0, placedSfx[index].Item3);
            }
        }

        public async Task BuildAsync(List<Point> wiredTiles, Point controllerTile, Point sfxTile, int magicTileId)
        {
            var bcCatalogRoot = GetBcCatalog();
            ICatalogPage GetBcPageNamed(string name) => GetBcCatalogPage(bcCatalogRoot.First(x => x.Name == name).Id);

            allNotes = GetAllNotes();
            var roomEffects = GetRoomEffects();
            bcCatalogPages = GetCatalogPages();
            var bcCatalogOffers = GetCatalogOffers();

            var allNotesInSong = GetCurrentSoundNotes();

            var notes = new List<List<(Note, (int, int, string))>>();

            await SendAsync(Out["MoveObject"], magicTileId, sfxTile.X, sfxTile.Y, 0);
            await Task.Delay(250);
            await SendAsync(Out["BuildersClubPlaceRoomItem"], bcCatalogPages["gaming_furni"].Id, bcCatalogOffers["wf_glowball"].Id, "", (int)controllerTile.X, (int)controllerTile.Y, 0);
            FloorItem controller = FloorItem.Parse(await ReceiveAsync(In["ObjectAdd"]));
            await Task.Delay(250);

            foreach (var note in MainWindow.FullSong)
            {
                var noteList = new List<(Note, (int, int, string))>();

                if (note.Item3 == "-")
                {
                    notes.Add(noteList);
                    continue;
                }

                foreach (var subNote in note.Item3.ToCharArray())
                {
                    if (allNotes.TryGetValue(subNote.ToString(), out var _note))
                    {
                        noteList.Add((_note, note));
                    }
                }

                notes.Add(noteList);
            }

            var wiredSquareIndex = 0;

            foreach (var note in allNotes)
            {
                if (!allNotesInSong.Contains(note.Key)) continue;

                if (!roomEffects[note.Value.Furni].Any(x => x.Location.Z.ToString() == (note.Value.Height == "0" ? "0" : (float.Parse(note.Value.Height) / 100).ToString())))
                {
                    ICatalogOffer effect = bcCatalogOffers[note.Value.Furni];
                    var location = GetSFXLocation(note.Value.Furni, sfxTile);
                    var height = int.Parse(note.Value.Height);

                    await SendAsync(Out["SetCustomStackingHeight"], magicTileId, height);
                    await Task.Delay(250);
                    await SendAsync(Out["BuildersClubPlaceRoomItem"], bcCatalogPages["sfx"].Id, effect.Id, "", (int)location.X, (int)location.Y, 0);
                    await Task.Delay(250);

                    note.Value.PlacedEffect = true;
                }

                roomEffects = GetRoomEffects();

                note.Value.Location = wiredTiles[wiredSquareIndex];
                wiredSquareIndex++;
            }

            var triggerPoints = new List<Point>();

            foreach (var noteInfoList in notes)
            {
                if (noteInfoList.Count == 0)
                {
                    continue;
                }

                foreach (var noteInfo in noteInfoList)
                {
                    var note = noteInfo.Item1;

                    if (!note.Placed)
                    {
                        note.Placed = true;
                        var soundEffect = roomEffects[note.Furni].First(x => x.Location.Z.ToString() == (note.Height == "0" ? "0" : (float.Parse(note.Height) / 100).ToString()));
                        var effect = await PlaceFurni(bcCatalogPages["wf_act"].Id, bcCatalogOffers["wf_act_toggle_state"].Id, (int)note.Location.X, (int)note.Location.Y);
                        await Task.Delay(250);

                        // Add effect to generate the sound
                        await SendAsync(Out["UpdateAction"], (int)effect.Id, 1, 0, "", 1, (int)soundEffect.Id, 0, 1, 100, 0, 0);
                        await Task.Delay(250);

                        triggerPoints.Add(note.Location);

                        // Add condition: any of the conditions
                        var _allNotes = notes.SelectMany(x => x).Select(x => x.Item2.Item3).ToList();

                        if (_allNotes.Count(x => x == noteInfo.Item2.Item3) > 1)
                        {
                            await SendAsync(Out["BuildersClubPlaceRoomItem"], bcCatalogPages["wf_add"].Id, bcCatalogOffers["wf_xtra_or_eval"].Id, "", (int)note.Location.X, (int)note.Location.Y, 0);
                        }
                    }

                    // Add condition: controller has altitude
                    await Task.Delay(250);
                    var _altitudeCondition = await PlaceFurni(bcCatalogPages["wf_cnd"].Id, bcCatalogOffers["wf_cnd_has_altitude"].Id, (int)note.Location.X, (int)note.Location.Y);
                    await SendAsync(Out["UpdateCondition"], (int)_altitudeCondition.Id, 2, noteInfo.Item2.Item1 / 50 + 1, 1, "", 1, (int)controller.Id, 0, 1, 100, 0, 0);
                    await Task.Delay(250);
                }
            }

            foreach (var point in triggerPoints)
            {
                // Add trigger to watch the controller every 50ms
                var _shortTrigger = await PlaceFurni(bcCatalogPages["wf_trg"].Id, bcCatalogOffers["wf_trg_period_short"].Id, (int)point.X, (int)point.Y);
                await SendAsync(Out["UpdateTrigger"], (int)_shortTrigger.Id, 1, 1, 0, 0, 0, "", 0);
                await Task.Delay(250);
            }

            await Task.Delay(500);

            // disableController
            var nextPoint = wiredTiles[wiredSquareIndex];
            wiredSquareIndex++;

            var shortTrigger = await PlaceFurni(bcCatalogPages["wf_trg"].Id, bcCatalogOffers["wf_trg_period_short"].Id, (int)nextPoint.X, (int)nextPoint.Y);
            await SendAsync(Out["UpdateTrigger"], (int)shortTrigger.Id, 1, 1, 0, 0, 0, "", 0);
            await Task.Delay(250);

            await SendAsync(Out["UseFurniture"], (int)controller.Id, 0);
            await Task.Delay(250);

            var stateAndPositionCondition = await PlaceFurni(bcCatalogPages["wf_cnd"].Id, bcCatalogOffers["wf_cnd_match_snapshot"].Id, (int)nextPoint.X, (int)nextPoint.Y, 500);
            await SendAsync(Out["UpdateCondition"], (int)stateAndPositionCondition.Id, 4, 1, 0, 0, 0, "", 1, (int)controller.Id, 0, 1, 100, 0, 0);

            var setFurniHeightEffect = await PlaceFurni(bcCatalogPages["wf_act"].Id, bcCatalogOffers["wf_act_set_altitude"].Id, (int)nextPoint.X, (int)nextPoint.Y, 500);
            await SendAsync(Out["UpdateAction"], (int)setFurniHeightEffect.Id, 2, 1, 0, "", 1, (int)controller.Id, 0, 1, 100, 0, 0);

            // enableController
            nextPoint = wiredTiles[wiredSquareIndex];
            wiredSquareIndex++;

            shortTrigger = await PlaceFurni(bcCatalogPages["wf_trg"].Id, bcCatalogOffers["wf_trg_period_short"].Id, (int)nextPoint.X, (int)nextPoint.Y);
            await SendAsync(Out["UpdateTrigger"], (int)shortTrigger.Id, 1, 1, 0, 0, 0, "", 0);
            await Task.Delay(250);

            await SendAsync(Out["UseFurniture"], (int)controller.Id, 0);
            await Task.Delay(250);

            stateAndPositionCondition = await PlaceFurni(bcCatalogPages["wf_cnd"].Id, bcCatalogOffers["wf_cnd_match_snapshot"].Id, (int)nextPoint.X, (int)nextPoint.Y, 500);
            await SendAsync(Out["UpdateCondition"], (int)stateAndPositionCondition.Id, 4, 1, 0, 0, 0, "", 1, (int)controller.Id, 0, 1, 100, 0, 0);
            await Task.Delay(250);

            var altitudeCondition = await PlaceFurni(bcCatalogPages["wf_cnd"].Id, bcCatalogOffers["wf_cnd_has_altitude"].Id, (int)nextPoint.X, (int)nextPoint.Y, 500);
            await SendAsync(Out["UpdateCondition"], (int)altitudeCondition.Id, 2, 0, 2, "", 1, (int)controller.Id, 0, 1, 100, 0, 0);
            await Task.Delay(500);

            altitudeCondition = await PlaceFurni(bcCatalogPages["wf_cnd"].Id, bcCatalogOffers["wf_cnd_has_altitude"].Id, (int)nextPoint.X, (int)nextPoint.Y, 500);
            await SendAsync(Out["UpdateCondition"], (int)altitudeCondition.Id, 2, (int)MainWindow.FullSong.Select(x => x.Item1).Max() / 50 + 1, 2, "", 1, (int)controller.Id, 0, 1, 100, 0, 0);
            await Task.Delay(500);

            var xtraOrEvalAddon = await PlaceFurni(bcCatalogPages["wf_add"].Id, bcCatalogOffers["wf_xtra_or_eval"].Id, (int)nextPoint.X, (int)nextPoint.Y, 500);
            await SendAsync(Out["UpdateAddon"], (int)xtraOrEvalAddon.Id, 3, 1, 0, 0, "", 2, (int)altitudeCondition.Id, (int)stateAndPositionCondition.Id, 1, 100, 0, 0);

            setFurniHeightEffect = await PlaceFurni(bcCatalogPages["wf_act"].Id, bcCatalogOffers["wf_act_set_altitude"].Id, (int)nextPoint.X, (int)nextPoint.Y, 500);
            await SendAsync(Out["UpdateAction"], (int)setFurniHeightEffect.Id, 2, 0, 2, "", 1, (int)controller.Id, 0, 1, 100, 0, 0);

            var actMatchEffect = await PlaceFurni(bcCatalogPages["wf_act"].Id, bcCatalogOffers["wf_act_match_to_sshot"].Id, (int)nextPoint.X, (int)nextPoint.Y, 500);
            await SendAsync(Out["UpdateAction"], (int)actMatchEffect.Id, 4, 1, 0, 0, 0, "", 1, (int)controller.Id, 0, 1, 100, 0, 0);
        }

        async Task<FloorItem> PlaceFurni(int catalogPageId, int catalogOfferId, int xPos, int yPos, int delay = 250)
        {
            await SendAsync(Out["BuildersClubPlaceRoomItem"], catalogPageId, catalogOfferId, "", xPos, yPos, 0);
            var result = FloorItem.Parse(await ReceiveAsync(In["ObjectAdd"]));
            await Task.Delay(delay);
            return result;
        }
    }
}