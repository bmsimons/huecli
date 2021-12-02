using System;
using System.Threading.Tasks;
using Xunit;
using HueCLI.Logic;
using HueCLI.Logic.Models;
using System.IO;

namespace HueCLI.Tests
{
    [TestCaseOrderer("HueCLI.Tests.PriorityOrderer", "HueCLI.Tests")]
    public class BridgeLinkTests
    {
        [Fact, TestPriority(1)]
        public async Task LinkBridge()
        {
            var bridgeLink = new BridgeLink();

            var username = await bridgeLink.Link(Settings.IPAddress, "UnitTest");

            var configurationStore = new ConfigurationStore();

            configurationStore.AddConfiguration(new Configuration
            {
                Id = 0,
                IPAddress = Settings.IPAddress,
                Username = username,
                Alias = "UnitTest"
            });

            var foundConfiguration = configurationStore.GetConfiguration(Settings.IPAddress);

            configurationStore.Dispose();

            Assert.True(foundConfiguration.Username == username);
        }
    }
}