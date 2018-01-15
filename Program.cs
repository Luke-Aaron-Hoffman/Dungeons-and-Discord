using System;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using Discord.Commands;

namespace DungeonsAndDiscord
{
    public class Program
    {
        static void Main(string[] args) => new Program().StartAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _discord;

        private CommandHandler _handler;

        public async Task StartAsync()
        {
            _discord = new DiscordSocketClient();
            //DON'T USE HARDCODED TOKEN YOU DUMBNUT
            await _discord.LoginAsync(TokenType.Bot, "THIS WOULD BE THE TOKEN");

            await _discord.StartAsync();

            _handler = new CommandHandler();

            await _handler.InitializeAsync(_discord);



            await Task.Delay(-1);
        }
    }

}
