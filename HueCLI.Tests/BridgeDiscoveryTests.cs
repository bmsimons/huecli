using System;
using System.Threading.Tasks;
using Xunit;
using HueCLI.Logic;

namespace HueCLI.Tests
{
    public class BridgeDiscoveryTests
    {
        [Fact]
        public async Task NetworkHasAtLeastOneBridge()
        {
            var bridgeDiscovery = new BridgeDiscovery();

            var bridges = await bridgeDiscovery.GetBridges();

            Assert.NotNull(bridges);
            Assert.True(bridges.Count > 0);
        }
    }
}
