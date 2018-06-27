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

            HueBridgeDiscovery hueBridgeDiscovery = new HueBridgeDiscovery();

            string mainAction = argParser.GetMainAction();
            // Console.WriteLine(mainAction);

            switch (mainAction)
            {
                case "scan-hubs":
                    Console.WriteLine("Scanning for hubs...");

                    List<HueBridgeObject> hueBridges = hueBridgeDiscovery.GetBridges();
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
                case "get-hubs":
                    foreach (var hub in settings.GetHubs())
                    {
                        Console.WriteLine("Hub "+hub["alias"]+" at "+hub["localipaddress"]);
                    }
                    break;
                case "remove-hub":
                    if (argParser.RemoveHubCheckEnoughArguments())
                    {
                        hueBridgeDiscovery.RemoveBridgeLink(settings.GetIPAddress(argParser.arguments[2]), settings.GetUsername(argParser.arguments[2]));
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

                        string bridgeUsername = hueBridgeDiscovery.CreateBridgeLink(hubToAdd["alias"], hubToAdd["localipaddress"]);
                        hubToAdd.Add("username", bridgeUsername);

                        settings.AddHub(hubToAdd);
                    }
                    else
                    {
                        Console.WriteLine("Invalid syntax, use huecli add-hub hubaliashere hubipaddresshere");
                    }
                    break;
                case "get-lighting":
                    if (argParser.GetLightingCheckEnoughArguments())
                    {
                        HueBridge hueBridge = new HueBridge(argParser.arguments[2], settings.GetIPAddress(argParser.arguments[2]), settings.GetUsername(argParser.arguments[2]));
                        hueBridge.GetBridgeLighting();
                    }
                    else
                    {
                        Console.WriteLine("Invalid syntax, use huecli get-lighting hubaliashere");
                    }
                    break;
                case "turn-on":
                    if (argParser.TurnOnOffCheckEnoughArguments())
                    {
                        HueBridge hueBridge = new HueBridge(argParser.arguments[2], settings.GetIPAddress(argParser.arguments[2]), settings.GetUsername(argParser.arguments[2]));
                        hueBridge.TurnOnLighting(argParser.arguments[3]);
                    }
                    else
                    {
                        Console.WriteLine("Invalid syntax, use huecli turn-on hubaliashere lightidhere");
                    }
                    break;
                case "turn-off":
                    if (argParser.TurnOnOffCheckEnoughArguments())
                    {
                        HueBridge hueBridge = new HueBridge(argParser.arguments[2], settings.GetIPAddress(argParser.arguments[2]), settings.GetUsername(argParser.arguments[2]));
                        hueBridge.TurnOffLighting(argParser.arguments[3]);
                    }
                    else
                    {
                        Console.WriteLine("Invalid syntax, use huecli turn-off hubaliashere lightidhere");
                    }
                    break;
                case "set-brightness":
                    if (argParser.SetBrightnessCheckEnoughArguments())
                    {
                        HueBridge hueBridge = new HueBridge(argParser.arguments[2], settings.GetIPAddress(argParser.arguments[2]), settings.GetUsername(argParser.arguments[2]));
                        hueBridge.SetLightingBrightness(argParser.arguments[3], argParser.arguments[4]);
                    }
                    else
                    {
                        Console.WriteLine("Invalid syntax, use huecli set-brightness hubaliashere lightidhere 1-254");
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
