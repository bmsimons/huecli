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
    public class BridgeDiscovery
    {
        public async Task<List<HueBridgeObject>> GetBridges()
        {
            var discoveryClient = new HttpClient();
            var discoveryResponse = await discoveryClient.GetAsync("https://discovery.meethue.com");

            if (discoveryResponse.IsSuccessStatusCode)
            {
                var responseContent = await discoveryResponse.Content.ReadAsStreamAsync();

                return await JsonSerializer.DeserializeAsync<List<HueBridgeObject>>(responseContent);
            }
            else
            {
                throw new BridgeDiscoveryHTTPStatusCodeException();
            }
        }

        public async Task<string> LinkToBridge(string IPAddress, string Alias)
        {
            var linkClient = new HttpClient();

            var bodyContent = new StringContent("{\"devicetype\":\"huecli#" + Alias + "\"}", Encoding.UTF8, "application/json");

            var linkResponse = await linkClient.PostAsync("http://"+IPAddress+"/api", bodyContent);

            if (linkResponse.IsSuccessStatusCode)
            {
                try
                {
                    var responseContent = await linkResponse.Content.ReadAsStreamAsync();

                    var errors = await JsonSerializer.DeserializeAsync<HueBridgeLinkError[]>(responseContent);
                    var error = errors.FirstOrDefault();

                    if (error == null)
                    {
                        throw new BridgeLinkUnknownException();
                    }

                    if (error.Data.Description == "link button not pressed")
                    {
                        throw new BridgeLinkButtonNotPressedException();
                    }
                    else
                    {
                        throw new BridgeLinkUnknownException(error.Data.Description);
                    }
                }
                catch (JsonException)
                {
                    try
                    {
                        var responseContent = await linkResponse.Content.ReadAsStreamAsync();

                        var success = await JsonSerializer.DeserializeAsync<HueBridgeLinkSuccess[]>(responseContent);

                        var successMessage = success.FirstOrDefault();

                        if (successMessage == null)
                        {
                            throw new BridgeLinkUnknownException();
                        }

                        return successMessage.Success.Username;
                    }
                    catch (JsonException)
                    {
                        throw new BridgeLinkUnknownException();
                    }
                }
            }
            else
            {
                throw new BridgeLinkHTTPStatusCodeException();
            }
            
            // TODO: Finish LinkToBridge.
        }
    }
}