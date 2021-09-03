using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;
using System.IO;

namespace KyteBot
{
    class Program
    {
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        string token = File.ReadAllText("token.ignore");
        public string prefixget = File.ReadAllText("prefix.txt");

        public async Task RunBotAsync()
        {
           Console.WriteLine(File.ReadAllText("prefix.txt"));
            _client = new DiscordSocketClient();
            _commands = new CommandService();

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton<InteractiveService>()
                .AddSingleton(_commands)
                .BuildServiceProvider();

            _client.Log += _client_Log;
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            _commands = new CommandService();
            await RegisterCommandsAsync();
            
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private Task _client_Log(LogMessage arg)
        {
            Console.WriteLine(File.ReadAllText("prefix.txt"));
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync()
        {
            Console.WriteLine(File.ReadAllText("prefix.txt"));
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            Console.WriteLine(File.ReadAllText("prefix.txt"));
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);
            if (message.Author.IsBot) return;

            int argPos = 0;
            if (message.HasStringPrefix(prefixget, ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess)
                {
                    Console.WriteLine(result.ErrorReason);
                }
                if (result.Error.Equals(CommandError.UnmetPrecondition))
                {
                    await message.Channel.SendMessageAsync(result.ErrorReason);
                }
            }
        }
    }

}
