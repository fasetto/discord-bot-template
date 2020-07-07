FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build
WORKDIR /app

COPY . .

WORKDIR /app/src/DiscordBotTemplate

RUN dotnet restore
RUN dotnet publish -c Release -o dist

FROM mcr.microsoft.com/dotnet/core/runtime:3.1 as runtime

WORKDIR /app

COPY --from=build /app/src/DiscordBotTemplate/dist .

ENTRYPOINT ["dotnet", "DiscordBotTemplate.dll"]
