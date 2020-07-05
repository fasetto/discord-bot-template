using Newtonsoft.Json;

namespace DiscordBotTemplate.Models
{
    public class BotConfig
    {
        [JsonProperty("Token")]
        public string Token { get; set; }

        [JsonProperty("DefaultPrefix")]
        public string DefaultPrefix { get; set; }

        [JsonProperty("GameStatus")]
        public string GameStatus { get; set; }

        [JsonProperty("InviteUrl")]
        public string InviteUrl { get; set; }
    }
}
