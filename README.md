## HAVE THE POWER TO COMMAND YOUR HUE BULBS FROM THE COMMAND LINE... AND WITHOUT ANY MESSY REQUESTS/HUEAPI CALLS, TOO!

I recently got my hands dirty with Philips Hue lighting, and the experience has been pretty good so far. There was still one thing that really got me interested: checking out the possibilities of the Philips Hue API. Since I am a frequent user of the command line, I decided to start working on a command line application to control my Hue lighting with, just for fun. I called it huecli and in this guide I will show you how you can get it to work with your Philips Hue setup.

Huecli is a work in progress project at the time of writing this article, and already contains functionality for the following use cases:

    Hub management: you can add, remove and use multiple Hue hubs in one utility.
    Turning lights on and off
    Setting brightness of lights

Of course, more functionality will be added to the source code later on.

# Building huecli

Huecli is made in C#, and requires the .NET Core framework to run it. Runtimes are available for macOS, Linux and Windows.

All code is available at https://github.com/bmsimons/huecli. After you cloned the repository, go to your terminal and change your directory to the cloned repo location. Run dotnet restore to restore all packages/dependencies. Now you can build executables for your platform:

# macOS
dotnet build -c Release -r osx-x64

# Linux
dotnet build -c Release -r linux-x64

# Windows
dotnet build -c Release -r win-x64

Produced binaries can be found in bin/Release/netcoreapp2.0/osx-x64. Of course, osx-x64 is my platform identifier, if you use Windows or Linux you should change that.

One handy trick is to add this location to your path environment variable, so that you don't have to type the full path to the binary everytime you want to use it.

On macOS it works like this:

PATH=$PATH:/Users/bart/Downloads/huecli/bin/Release/netcoreapp2.0/osx-x64

Type this in your terminal (replace the path of course) and you should be good to go.

huecli-help

If the command huecli returns a list of available commands, you are good to go!
Scanning for available hubs using UPnP

Use the command huecli scan-hubs to get all available hubs:

scan-bridge

Please note that this is purely for discovering hubs - this does not add your hub to the program yet. Also, this might not work if your router doesn't have UPnP. In that case you need to manually find your hub IP address to add it later on.
Adding your hub to the program

Use the command huecli add-hub hubaliashere hubipaddresshere to add your hub. The hub alias is a nickname for your hub which serves as an identifier (so it has to be unique).

So an example would be:

add-bridge

As you can see, all you have to do now is press the physical button on the top of your hub. Your hub should now be linked to your app.
Getting a list of lights

Want to see a list of light names with their IDs? You can use huecli get-lighting hubaliasgoeshere for that:

huecli-get-lighting

You'll need the light IDs for use with the light control commands. More on that in all the paragraphs below.
Turning on a light (and turning it back off)

Turning on a light is fairly easy if you can remember your light IDs. This is how I turn my bed light on:

huecli-lighting-turn-on

You can replace turn-on with turn-off if you want to turn your light off.
Setting the brightness of a light

You can easily set the brightness of a specific light with the command as shown in the picture here:

huecli-lighting-set-brightness

mainhub is the alias of my hub, 1 is the light ID and 254 is the brightness level between a scale of 1 to 254.
Aftermath

Cool! Now that you can control your Hue lighting from the CLI. What's next? I am planning to implement other functionality like rooms, entertainment areas, light warmth and color in the future. The best way to stay up to date is to star and follow my GitHub repo at https://github.com/bmsimons/huecli. I hope you have learned something and as always, have a nice day 😉
