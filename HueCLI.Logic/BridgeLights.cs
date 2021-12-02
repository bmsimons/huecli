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
        private string _ipAddress { get; set; }

        public BridgeLights(string IPAddress) {
            _ipAddress = IPAddress;
        }

        public async Task<Dictionary<int, HueBridgeLight>> GetLights()
        {
            var configurationStore = new ConfigurationStore();

            var configuration = configurationStore.GetConfiguration(_ipAddress);

            var webClient = new HttpClient();

            var webResponse = await webClient.GetAsync("http://" + _ipAddress + "/api/" + configuration.Username + "/lights");

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

        public async Task<bool> TurnOn(int light) {
            return await TurnOnOrOff(light, true);
        }

        public async Task<bool> TurnOff(int light) {
            return await TurnOnOrOff(light, false);
        }

        public async Task<bool> SetBrightness(int light, int brightness) {
            if (brightness < 0 || brightness > 254) {
                throw new BridgeLightHTTPStatusCodeException();
            }

            var configurationStore = new ConfigurationStore();

            var configuration = configurationStore.GetConfiguration(_ipAddress);

            var webClient = new HttpClient();

            var bodyContent = new StringContent("{\"bri\": " + brightness + " }", Encoding.UTF8, "application/json");

            var webResponse = await webClient.PutAsync("http://" + _ipAddress + "/api/" + configuration.Username + "/lights/" + light + "/state", bodyContent);

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

        public async Task<bool> SetColorTemperature(int light, int temperature) {
            if (temperature > 500 || temperature < 154) {
                throw new BridgeLightHTTPStatusCodeException();
            }

            var configurationStore = new ConfigurationStore();

            var configuration = configurationStore.GetConfiguration(_ipAddress);

            var webClient = new HttpClient();

            var bodyContent = new StringContent("{\"ct\": " + temperature + " }", Encoding.UTF8, "application/json");

            var webResponse = await webClient.PutAsync("http://" + _ipAddress + "/api/" + configuration.Username + "/lights/" + light + "/state", bodyContent);

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

        private async Task<bool> TurnOnOrOff(int light, bool on) {
            var configurationStore = new ConfigurationStore();

            var configuration = configurationStore.GetConfiguration(_ipAddress);

            var webClient = new HttpClient();

            var bodyContent = new StringContent("{\"on\": " + on.ToString().ToLower() + " }", Encoding.UTF8, "application/json");

            var webResponse = await webClient.PutAsync("http://" + _ipAddress + "/api/" + configuration.Username + "/lights/" + light + "/state", bodyContent);

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