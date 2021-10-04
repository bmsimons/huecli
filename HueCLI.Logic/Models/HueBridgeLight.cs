using System.Text.Json.Serialization;

namespace HueCLI.Logic
{
    public class HueBridgeLight
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("productname")]
        public string ProductName { get; set; }
    }
}