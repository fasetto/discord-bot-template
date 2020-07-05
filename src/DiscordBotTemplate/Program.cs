using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBotTemplate.Models;
using DiscordBotTemplate.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace DiscordBotTemplate
{
    class Program
    {
        private static DiscordSocketClient client;
        private static BotConfig config;

        static async Task Main(string[] args)
        {
            client = new DiscordSocketClient();
            config = ReadConfigFile();

            var services = ConfigureServices();

            services.GetRequiredService<LogService>();

            await services.GetRequiredService<CommandHandlingService>().InitializeAsync(services);

            await client.LoginAsync(TokenType.Bot, config.Token);
            await client.SetGameAsync(config.GameStatus, type: ActivityType.Watching);
            await client.StartAsync();

            await Task.Delay(-1);
        }

        private static IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                // Base
                .AddSingleton(client)
                .AddSingleton(new CommandService(new CommandServiceConfig()
                {
                    DefaultRunMode = RunMode.Async,
                }))
                .AddSingleton<CommandHandlingService>()

                // Logging
                .AddSingleton<LogService>()

                // Extra
                .AddSingleton(config)

                .BuildServiceProvider();
        }

        private static BotConfig ReadConfigFile()
        {

            var path = "./config.json";

            #if DEBUG

            path = "../../config.json";

            #endif

            var configStr = File.ReadAllText(path);
            var config    = JsonConvert.DeserializeObject<BotConfig>(configStr);

            return config;
        }
    }
}
