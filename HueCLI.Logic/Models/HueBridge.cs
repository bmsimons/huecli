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


    public class HueBridgeLinkError
    {
        [JsonPropertyName("error")]
        public HueBridgeLinkErrorValue Data { get; set; }
    }

    public class HueBridgeLinkErrorValue
    {

        [JsonPropertyName("type")]
        public int Type { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class HueBridgeLinkSuccessUsername
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }
    }

    public class HueBridgeLinkSuccess
    {
        [JsonPropertyName("success")]
        public HueBridgeLinkSuccessUsername Success { get; set; }
    }
}