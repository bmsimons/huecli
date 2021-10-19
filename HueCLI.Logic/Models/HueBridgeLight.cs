using System.Text.Json.Serialization;

namespace HueCLI.Logic
{
    public class HueBridgeLightState
    {
        [JsonPropertyName("on")]
        public bool On { get; set; }

        [JsonPropertyName("reachable")]
        public bool Reachable { get; set; }

        [JsonPropertyName("bri")]
        public int Brightness { get; set; }

        [JsonPropertyName("ct")]
        public int ColorTemperature { get; set; }

        [JsonPropertyName("hue")]
        public int Hue { get; set; }

        [JsonPropertyName("sat")]
        public int Saturation { get; set; }
    }

    public class HueBridgeLight
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("productname")]
        public string ProductName { get; set; }

        [JsonPropertyName("state")]
        public HueBridgeLightState State { get; set; }
    }
}