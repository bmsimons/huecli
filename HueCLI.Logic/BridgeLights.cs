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
        public async Task<Dictionary<int, HueBridgeLight>> GetLights(string IPAddress)
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
                    return await JsonSerializer.DeserializeAsync<Dictionary<int, HueBridgeLight>>(responseContent);
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

        public async Task<bool> TurnOn(string IPAddress, int light) {
            return await TurnOnOrOff(IPAddress, light, true);
        }

        public async Task<bool> TurnOff(string IPAddress, int light) {
            return await TurnOnOrOff(IPAddress, light, false);
        }

        private async Task<bool> TurnOnOrOff(string IPAddress, int light, bool on) {
            var configurationStore = new ConfigurationStore();

            var configuration = configurationStore.GetConfiguration(IPAddress);

            var webClient = new HttpClient();

            var bodyContent = new StringContent("{\"on\": " + on.ToString().ToLower() + " }", Encoding.UTF8, "application/json");

            var webResponse = await webClient.PutAsync("http://" + IPAddress + "/api/" + configuration.Username + "/lights/" + light + "/state", bodyContent);

            if (webResponse.IsSuccessStatusCode)
            {
                var responseContent = await webResponse.Content.ReadAsStreamAsync();

                try
                {
                    var errors = await JsonSerializer.DeserializeAsync<HueBridgeLinkError[]>(responseContent);
                    var error = errors.FirstOrDefault();

                    if (error == null || error.Data == null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (JsonException)
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
    }
}