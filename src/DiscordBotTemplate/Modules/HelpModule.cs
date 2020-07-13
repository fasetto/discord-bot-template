using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using DiscordBotTemplate.Models;

namespace DiscordBotTemplate.Modules
{
    [Name("Help Module")]
    public class HelpModule: ModuleBase<SocketCommandContext>
    {
        private readonly CommandService service;
        private readonly BotConfig config;

        public HelpModule(CommandService service, BotConfig config)
        {
            this.service = service;
            this.config  = config;
        }

        public async Task Help()
        {
            var prefix = config.DefaultPrefix;
            var builder = new EmbedBuilder()
            {
                Color = new Color(0xFD3439),
                Description = ":closed_book: These are the commands you can use"
            };

            foreach (var module in service.Modules)
            {
                string description = "";

                foreach (var cmd in module.Commands)
                {
                    description += $"**{prefix}{cmd.Aliases.FirstOrDefault()}**\n";
                    description += cmd.Summary + "\n\n";
                }

                if (!string.IsNullOrWhiteSpace(description))
                {
                    builder.AddField(x =>
                    {
                        x.Name     = module.Name;
                        x.Value    = description;
                        x.IsInline = false;
                    });
                }
            }

            await ReplyAsync("", embed: builder.Build());
        }

        [Command("help")]
        [Summary("Prints this help message.")]
        public async Task Help([Remainder] string command = null)
        {
            if (command == null)
            {
                await Help();
                return;
            }

            var result = service.Search(Context, command);

            if (!result.IsSuccess)
            {
                await ReplyAsync($":expressionless: I couldn't find that command.");
                return;
            }

            var prefix = config.DefaultPrefix;

            var builder = new EmbedBuilder()
            {
                Color       = new Color(0xFD3439),
                Description = $":closed_book: Here are some commands like **{command}**"
            };

            foreach (var match in result.Commands)
            {
                var cmd = match.Command;

                builder.AddField(x =>
                {
                    x.Name = string.Join(", ", cmd.Aliases);
                    x.Value = $"Parameters: {string.Join(", ", cmd.Parameters.Select(p => p.Name))}\n" +
                              $"Summary   : {cmd.Summary}";
                    x.IsInline = false;
                });
            }

            await ReplyAsync("", embed: builder.Build());
        }

        [Command("invite")]
        [Summary("Prints the invite code of the bot.")]
        public async Task Invite()
        {
            var invite = config.InviteUrl;

            await Context.User.SendMessageAsync(invite);
            await ReplyAsync(":white_check_mark: Invite link sent via DM.");
        }
    }
}
