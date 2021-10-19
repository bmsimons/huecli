using System.Linq;
using System.Threading.Tasks;
using Xunit;
using HueCLI.Logic;
using System.IO;
using HueCLI.Logic.Models;

namespace HueCLI.Tests
{
    [TestCaseOrderer("HueCLI.Tests.PriorityOrderer", "HueCLI.Tests")]
    public class BridgeLightsTests {
        [Fact, TestPriority(1)]
        public void PrepareDB() {
            if (File.Exists("huecli.db"))
            {
                File.Delete("huecli.db");
            }

            if (File.Exists("huecli-log.db"))
            {
                File.Delete("huecli-log.db");
            }

            ConfigurationStore store = new ConfigurationStore();
            store.Dispose();

            Assert.True(File.Exists("huecli.db"));
        }

        [Fact, TestPriority(2)]
        public async Task WriteUsernameToDB() {
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

        [Fact, TestPriority(3)]
        public async Task TestLightsGet() {
            var bridgeLights = new BridgeLights();

            var lights = await bridgeLights.GetLights(Settings.IPAddress);
            
            Assert.True(Settings.LightNames.All(a => lights.Values.Select(s => s.Name).Contains(a)));
            Assert.True(Settings.LightProductNames.All(a => lights.Values.Select(s => s.ProductName).Contains(a)));
        }

        [Fact, TestPriority(4)]
        public async Task TestTurnOffLight() {
            var bridgeLights = new BridgeLights();

            Assert.True(await bridgeLights.TurnOff(Settings.IPAddress, Settings.LightToTurnOffAndOn));
        }

        [Fact, TestPriority(5)]
        public async Task TestTurnOnLight() {
            var bridgeLights = new BridgeLights();

            Assert.True(await bridgeLights.TurnOn(Settings.IPAddress, Settings.LightToTurnOffAndOn));
        }
    }
}