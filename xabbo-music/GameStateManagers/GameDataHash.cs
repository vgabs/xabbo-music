using System.Text.Json.Serialization;

namespace xabbo_music.GameStateManagers
{
    internal class GameDataHash
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        [JsonPropertyName("hash")]
        public string Hash { get; set; } = string.Empty;
    }
}
