using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscordBotTemplate.Services
{
    public class LogService
    {
        private readonly DiscordSocketClient discord;
        private readonly CommandService commands;

        public LogService(DiscordSocketClient discord, CommandService commands)
        {
            this.discord  = discord;
            this.commands = commands;

            this.discord.Log  += LogDiscord;
            this.commands.Log += LogCommand;
        }

        private Task LogDiscord(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }

        private Task LogCommand(LogMessage message)
        {
            // Return an error message for async commands
            if (message.Exception is CommandException command)
                // Don't risk blocking the logging task by awaiting a message send; ratelimits!?
                command.Context.Channel.SendMessageAsync($":expressionless: {command.InnerException.Message}");

            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }
    }
}
