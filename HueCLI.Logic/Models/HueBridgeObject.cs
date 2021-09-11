using System.Text.Json.Serialization;

namespace HueCLI.Logic.Models
{
    public class HueBridgeObject
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("internalipaddress")]
        public string InternalIPAddress { get; set; }
    }
}