using System.Text.Json.Serialization;

namespace HueCLI.Logic
{
    public class HueBridgeLightColorGamut
    {
        public readonly XYPoint Red;
        public readonly XYPoint Green;
        public readonly XYPoint Blue;

        public HueBridgeLightColorGamut(XYPoint red, XYPoint green, XYPoint blue) {
            Red = red;
            Green = green;
            Blue = blue;
        }
    }

    public class HueBridgeLightCapabilitiesControl
    {
        [JsonPropertyName("colorgamuttype")]
        public string ColorGamutType { get; set; }

        [JsonPropertyName("colorgamut")]
        [JsonConverter(typeof(GamutJsonConverter))]
        public HueBridgeLightColorGamut ColorGamut { get; set; }
    }

    public class HueBridgeLightCapabilities
    {
        [JsonPropertyName("certified")]
        public bool Certified { get; set; }

        [JsonPropertyName("control")]
        public HueBridgeLightCapabilitiesControl Control { get; set; }
    }

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
        
        [JsonPropertyName("capabilities")]
        public HueBridgeLightCapabilities Capabilities { get; set; }
    }
}