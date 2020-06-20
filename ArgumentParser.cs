using System;
using System.Collections.Generic;

namespace huecli
{
    public class ArgumentParser
    {
        public String[] arguments { get; set; }

        public void ShowHelp()
        {
            Console.WriteLine("huecli 1.0 - made by bmsimons, 2018");
            Console.WriteLine("Control your Hue lighting/home automation from the CLI.");
            Console.WriteLine("");
            Console.WriteLine("Arguments:");
            Console.WriteLine("");
            Console.WriteLine("Scan for UPnP-enabled hubs:     huecli scan-hubs");
            Console.WriteLine("Add a hub:                      huecli add-hub");
            Console.WriteLine("Remove a hub:                   huecli remove-hub");
            Console.WriteLine("Get available hubs:             huecli get-hubs");
            Console.WriteLine("Get avail. lighting for hub:    huecli get-lighting hubalias");
            Console.WriteLine("");
            Console.WriteLine("Turn light on:                  huecli turn-on hubalias lightid");
            Console.WriteLine("Turn light off:                 huecli turn-off hubalias lightid");
            Console.WriteLine("Set light brightness:           huecli set-brightness hubalias lightid 1-254");
            Console.WriteLine("Set light temperature:          huecli set-temperature hubalias lightid 154-500");
        }

        public bool ShouldShowHelp()
        {
            if (this.arguments.Length > 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public string GetMainAction()
        {
            if (this.ShouldShowHelp())
            {
                return null;
            }
            else
            {
                return this.arguments[1];
            }
        }

        public bool RemoveHubCheckEnoughArguments()
        {
            if (this.arguments.Length == 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddHubCheckEnoughArguments()
        {
            if (this.arguments.Length == 4)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GetLightingCheckEnoughArguments()
        {
            if (this.arguments.Length == 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TurnOnOffCheckEnoughArguments()
        {
            if (this.arguments.Length == 4)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SetBrightnessCheckEnoughArguments()
        {
            if (this.arguments.Length == 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SetTemperatureCheckEnoughArguments()
        {
            if (this.arguments.Length == 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SetColorCheckEnoughArguments()
        {
            if (this.arguments.Length == 6)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ArgumentParser()
        {
            arguments = Environment.GetCommandLineArgs();
        }
    }
}