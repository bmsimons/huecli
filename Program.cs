using System;
using System.Collections.Generic;

namespace huecli
{
    class Program
    {
        private static HueBridge activeHueBridge { get; set; }
        private static ArgumentParser argParser { get; set; }
        private static Settings settings { get; set; }
        private static Preferences preferences { get; set; }

        static void Main(string[] args)
        {
            settings = new Settings();
            preferences = new Preferences();

            argParser = new ArgumentParser();

            string mainAction = argParser.GetMainAction();
            Console.WriteLine(mainAction);

            switch (mainAction)
            {
                case "scan-hubs":
                    Console.WriteLine("Scanning for hubs...");

                    List<HueBridgeObject> hueBridges = new HueBridgeDiscovery().GetBridges();
                    if (hueBridges.Count != 0)
                    {
                        foreach (HueBridgeObject hueBridge in hueBridges)
                        {
                            Console.WriteLine("Found hub at address "+hueBridge.internalipaddress);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No hubs could be discovered unfortunately.");
                    }

                    break;
                case "get-hub":
                    Console.WriteLine("Your current hub is not yet set.");
                    break;
                case "get-hubs":
                    foreach (var hub in settings.GetHubs())
                    {
                        Console.WriteLine("Hub "+hub["alias"]+" at "+hub["localipaddress"]);
                    }
                    break;
                case "set-hub":
                    if (argParser.SetHubCheckEnoughArguments())
                    {
                        activeHueBridge = new HueBridge(argParser.arguments[2], argParser.arguments[3]);
                    }
                    else
                    {
                        Console.WriteLine("Incorrect syntax, use huecli set-hub MainHub 10.0.1.30");
                    }
                    break;
                case "remove-hub":
                    if (argParser.RemoveHubCheckEnoughArguments())
                    {
                        settings.RemoveHub(argParser.arguments[2]);
                    }
                    else
                    {
                        Console.WriteLine("Invalid syntax, use huecli remove-hub hubaliasgoeshere");
                    }
                    break;
                case "add-hub":
                    if (argParser.AddHubCheckEnoughArguments())
                    {
                        Dictionary<string, string> hubToAdd = new Dictionary<string, string>();
                        hubToAdd.Add("alias", argParser.arguments[2]);
                        hubToAdd.Add("localipaddress", argParser.arguments[3]);

                        settings.AddHub(hubToAdd);
                    }
                    else
                    {
                        Console.WriteLine("Invalid syntax, use huecli add-hub hubaliashere hubipaddresshere");
                    }
                    break;
                default:
                    argParser.ShowHelp();
                    break;
            }

            settings.Save();
        }
    }
}
