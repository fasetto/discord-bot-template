using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBotTemplate.Models;

namespace DiscordBotTemplate.Services
{
    public class CommandHandlingService
    {
        private readonly DiscordSocketClient discord;
        private readonly CommandService commands;
        private IServiceProvider provider;
        private readonly BotConfig config;

        public CommandHandlingService(IServiceProvider provider, DiscordSocketClient discord, CommandService commands, BotConfig config)
        {
            this.discord  = discord;
            this.commands = commands;
            this.provider = provider;
            this.config   = config;

            this.discord.MessageReceived += MessageReceived;
        }

        public async Task InitializeAsync(IServiceProvider provider)
        {
            this.provider = provider;
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), provider);
            // Add additional initialization code here...
        }

        private async Task MessageReceived(SocketMessage rawMessage)
        {
            // Ignore system messages and messages from bots
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            var context = new SocketCommandContext(discord, message);
            var prefix  = config.DefaultPrefix;

            int argPos = 0;

            // if (!message.HasMentionPrefix(_discord.CurrentUser, ref argPos)) return;
            if (!message.HasStringPrefix(prefix, ref argPos)) return;

            var result = await commands.ExecuteAsync(context, argPos, provider);

            if (result.IsSuccess)
                return;

            await context.Channel.SendMessageAsync(result.ErrorReason);
        }

    }
}
