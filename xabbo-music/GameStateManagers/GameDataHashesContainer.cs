using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace xabbo_music.GameStateManagers
{
    internal class GameDataHashesContainer
    {
        [JsonPropertyName("hashes")]
        public List<GameDataHash> Hashes { get; set; } = new();
    }
}