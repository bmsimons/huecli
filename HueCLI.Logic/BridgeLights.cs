using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HueCLI.Logic.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;
using System.Linq;

namespace HueCLI.Logic
{
    public class BridgeLights
    {
        public async Task<Dictionary<string, HueBridgeLight>> GetLights(string IPAddress)
        {
            var configurationStore = new ConfigurationStore();

            var configuration = configurationStore.GetConfiguration(IPAddress);

            var webClient = new HttpClient();

            var webResponse = await webClient.GetAsync("http://" + IPAddress + "/api/" + configuration.Username + "/lights");

            if (webResponse.IsSuccessStatusCode)
            {
                var responseContent = await webResponse.Content.ReadAsStreamAsync();

                try
                {
                    return await JsonSerializer.DeserializeAsync<Dictionary<string, HueBridgeLight>>(responseContent);
                }
                catch (JsonException)
                {
                    throw new BridgeLightUnknownException();
                }
            }
            else
            {
                throw new BridgeLightHTTPStatusCodeException();
            }
        }
    }
}