using System;
using System.Text;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        public bool RemoveBridgeLink(String localipaddress, String username)
        {
            HttpClient apiClient = new HttpClient();
            HttpResponseMessage responseMessage = apiClient.DeleteAsync("http://"+localipaddress+"/api/"+username+"/config/whitelist/"+username).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    class HueBridge
    {
        public string HueBridgeAddress  { get; set; }
        public string HueBridgeAlias    { get; set; }
        public string HueBridgeUsername { get; set; }

        // public HueBridge(String hueBridgeAlias, String hueBridgeAddress = null)
        // {
        //     HueBridgeAlias = hueBridgeAlias;

        //     if (hueBridgeAddress == null)
        //     {
        //         List<HueBridgeObject> hueBridges = new HueBridgeDiscovery().GetBridges();

        //         foreach (HueBridgeObject hueBridge in hueBridges) {
        //             Console.WriteLine("IP Address: "+hueBridge.internalipaddress);
        //         }
        //     }
        //     else
        //     {
        //         HueBridgeAddress = hueBridgeAddress;
        //     }
        // }

        public HueBridge(String hueBridgeAlias, String hueBridgeAddress, String hueBridgeUsername)
        {
            this.HueBridgeAddress  = hueBridgeAddress;
            this.HueBridgeAlias    = hueBridgeAlias;
            this.HueBridgeUsername = hueBridgeUsername;
        }

        public void GetBridgeLighting()
        {
            HttpClient apiClient = new HttpClient();
            HttpResponseMessage responseMessage = apiClient.GetAsync("http://"+this.HueBridgeAddress+"/api/"+this.HueBridgeUsername).Result;
            String returnedMessage = responseMessage.Content.ReadAsStringAsync().Result;
            var jsonObject = JObject.Parse(returnedMessage);
            foreach (JProperty lightObject in jsonObject["lights"])
            {
                Console.WriteLine("Light ID "+lightObject.Name+" with name "+lightObject.First["name"]+" found.");
            }
        }

        public void TurnOnLighting(String lightID)
        {
            HttpClient apiClient = new HttpClient();
            StringContent content = new StringContent("{\"on\": true}", Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = apiClient.PutAsync("http://"+this.HueBridgeAddress+"/api/"+this.HueBridgeUsername+"/lights/"+lightID+"/state", content).Result;
        }

        public void TurnOffLighting(String lightID)
        {
            HttpClient apiClient = new HttpClient();
            StringContent content = new StringContent("{\"on\": false}", Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = apiClient.PutAsync("http://"+this.HueBridgeAddress+"/api/"+this.HueBridgeUsername+"/lights/"+lightID+"/state", content).Result;
        }

        public void SetLightingBrightness(String lightID, String brightness)
        {
            HttpClient apiClient = new HttpClient();
            StringContent content = new StringContent("{\"bri\": "+brightness+"}", Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = apiClient.PutAsync("http://"+this.HueBridgeAddress+"/api/"+this.HueBridgeUsername+"/lights/"+lightID+"/state", content).Result;
        }

        public void SetLightingTemperature(String lightID, String temperature)
        {
            HttpClient apiClient = new HttpClient();
            StringContent content = new StringContent("{\"ct\": "+temperature+"}", Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = apiClient.PutAsync("http://"+this.HueBridgeAddress+"/api/"+this.HueBridgeUsername+"/lights/"+lightID+"/state", content).Result;
        }
    }
}