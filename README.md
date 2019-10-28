## Installation
After you cloned the repository, go to your terminal and change your directory to the cloned repo location. Run dotnet restore to restore all packages/dependencies. Now you can build executables for your platform:

### macOS
`dotnet build -c Release -r osx-x64`

### Linux
`dotnet build -c Release -r linux-x64`

### Windows
`dotnet build -c Release -r win-x64`
