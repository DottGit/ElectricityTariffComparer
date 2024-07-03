This was a little coding challenge, i created this project myself. I used tdd as a tool to react on failure early on.

installscript for linux:

# Install .NET SDK if not installed
if ! command -v dotnet &> /dev/null
then
    echo ".NET SDK not found, installing..."
    wget https://dotnet.microsoft.com/download/dotnet/thank-you/sdk-6.0.100-linux-x64-binaries
    tar -zxf dotnet-sdk-6.0.100-linux-x64-binaries.tar.gz -C $HOME/dotnet
    export DOTNET_ROOT=$HOME/dotnet
    export PATH=$PATH:$HOME/dotnet
else
    echo ".NET SDK is already installed."
fi

#install packages
dotnet add package AutoMapper

# Restore and build the project
dotnet restore
dotnet build
