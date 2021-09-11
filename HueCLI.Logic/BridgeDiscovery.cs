using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HueCLI.Logic.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

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
                throw new UnexpectedBridgeDiscoveryResultException();
            }
        }
    }
}