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
    public class BridgeLink
    {
        public async Task<string> Link(string IPAddress, string Alias)
        {
            var linkClient = new HttpClient();

            var bodyContent = new StringContent("{\"devicetype\":\"huecli#" + Alias + "\"}", Encoding.UTF8, "application/json");

            var linkResponse = await linkClient.PostAsync("http://"+IPAddress+"/api", bodyContent);

            if (linkResponse.IsSuccessStatusCode)
            {
                var responseContent = await linkResponse.Content.ReadAsStreamAsync();

                try
                {
                    responseContent.Position = 0;

                    var errors = await JsonSerializer.DeserializeAsync<HueBridgeLinkError[]>(responseContent);
                    var error = errors.FirstOrDefault();

                    if (error == null || error.Data == null)
                    {
                        throw new JsonException();
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
                        responseContent.Position = 0;

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
        }
    }
}