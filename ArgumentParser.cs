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
            Console.WriteLine("Get the active hub(s):          huecli get-hub | huecli get-hubs");
            Console.WriteLine("Set the active hub:             huecli set-hub");
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

        public bool SetHubCheckEnoughArguments()
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

        public ArgumentParser()
        {
            arguments = Environment.GetCommandLineArgs();
        }
    }
}