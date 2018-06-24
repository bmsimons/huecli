using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace huecli
{
    public class HueBridgeObject
    {
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("internalipaddress")]
        public string internalipaddress { get; set; }
    }

    class HueBridgeDiscovery
    {
        public List<HueBridgeObject> GetBridges()
        {
            List<HueBridgeObject> hueBridges = new List<HueBridgeObject>(); 

            HttpClient nupnpClient = new HttpClient();
            HttpResponseMessage responseMessage = nupnpClient.GetAsync("https://www.meethue.com/api/nupnp").Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = responseMessage.Content;

                string responseString = responseContent.ReadAsStringAsync().Result;

                hueBridges = JsonConvert.DeserializeObject<List<HueBridgeObject>>(responseString);
            }

            return hueBridges;
        }
    }

    class HueBridge
    {
        public string HueBridgeAddress { get; set; }
        public string HueBridgeAlias   { get; set; }

        public HueBridge(String hueBridgeAlias, String hueBridgeAddress = null)
        {
            HueBridgeAlias = hueBridgeAlias;

            if (hueBridgeAddress == null)
            {
                List<HueBridgeObject> hueBridges = new HueBridgeDiscovery().GetBridges();

                foreach (HueBridgeObject hueBridge in hueBridges) {
                    Console.WriteLine("IP Address: "+hueBridge.internalipaddress);
                }

                // HttpClient nupnpClient = new HttpClient();
                // HttpResponseMessage responseMessage = nupnpClient.GetAsync("https://www.meethue.com/api/nupnp").Result;
                // if (responseMessage.IsSuccessStatusCode)
                // {
                //     var responseContent = responseMessage.Content;

                //     string responseString = responseContent.ReadAsStringAsync().Result;

                //     List<HueBridgeObject> hueBridges = JsonConvert.DeserializeObject<List<HueBridgeObject>>(responseString);

                //     foreach (HueBridgeObject hueBridge in hueBridges)
                //     {
                //         HueBridgeAddress = hueBridge.internalipaddress
                //         Console.WriteLine(hueBridge.internalipaddress);
                //     }
                // }
            }
            else
            {
                HueBridgeAddress = hueBridgeAddress;
            }
        }
    }
}