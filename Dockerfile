FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build
WORKDIR /app

COPY . .

WORKDIR /app/src/DiscordBotTemplate

RUN dotnet restore
RUN dotnet publish -c Release -o dist

WORKDIR /app/src/DiscordBotTemplate/dist

ENTRYPOINT ["dotnet", "DiscordBotTemplate.dll"]
