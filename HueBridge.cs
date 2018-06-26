using System;
using System.Text;
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

    public class HueBridgeLinkSuccessUsername
    {
        [JsonProperty("username")]
        public string username { get; set; }
    }

    public class HueBridgeLinkSuccess
    {
        [JsonProperty("success")]
        public HueBridgeLinkSuccessUsername success { get; set; }
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

        public bool DoesBridgeLinkExist(String alias, String localipaddress)
        {
            HttpClient apiClient = new HttpClient();
            HttpResponseMessage responseMessage = apiClient.GetAsync("http://"+localipaddress+"/api/"+alias).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                if (responseMessage.Content.ReadAsStringAsync().Result.Contains("[{\"error\":"))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public string CreateBridgeLink(String alias, String localipaddress)
        {
            HttpClient apiClient = new HttpClient();
            StringContent content = new StringContent("{\"devicetype\": \"huecli#"+alias+"\"}", Encoding.UTF8, "application/json");
            while (true)
            {
                HttpResponseMessage responseMessage = apiClient.PostAsync("http://"+localipaddress+"/api", content).Result;
                String returnedMessage = responseMessage.Content.ReadAsStringAsync().Result;
                if (!returnedMessage.Contains("\"description\":\"link button not pressed\"}}]"))
                {
                    // Console.WriteLine(returnedMessage);
                    return JsonConvert.DeserializeObject<HueBridgeLinkSuccess[]>(returnedMessage)[0].success.username;
                }
                else
                {
                    Console.WriteLine("Please press the link button on your Hue bridge..");
                }
                System.Threading.Thread.Sleep(5000);
            }
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