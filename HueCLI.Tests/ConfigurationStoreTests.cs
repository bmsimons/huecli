using System.Threading.Tasks;
using Xunit;
using HueCLI.Logic;
using System.IO;
using HueCLI.Logic.Models;

namespace HueCLI.Tests
{
    [TestCaseOrderer("HueCLI.Tests.PriorityOrderer", "HueCLI.Tests")]
    public class ConfigurationStoreTests
    {
        [Fact, TestPriority(1)]
        public void TestDBCreate()
        {
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
        public void TestIfAddConfigurationSucceeds()
        {
            ConfigurationStore store = new ConfigurationStore();

            Configuration configuration = new Configuration
            {
                Id = 0,
                IPAddress = Settings.IPAddress,
                Username = "7c291086-503c-43dc-98e1-f05c93013dd4",
                Alias = "hue_unit_test#huecli"
            };

            store.AddConfiguration(configuration);

            var foundConfiguration = store.GetConfiguration(Settings.IPAddress);

            Assert.True(foundConfiguration.Id == 1);
            Assert.True(foundConfiguration.IPAddress == configuration.IPAddress);
            Assert.True(foundConfiguration.Username == configuration.Username);
            Assert.True(foundConfiguration.Alias == configuration.Alias);

            store.Dispose();

            File.Delete("huecli.db");
            File.Delete("huecli-log.db");
        }
    }
}