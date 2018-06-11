
using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using RadDB3.interaction;

namespace DiscordBot {
	public class CommandHandlingService {
		private readonly CommandService _commands;
		private readonly DiscordSocketClient _discord;
		private readonly IServiceProvider _services;
		public static SocketCommandContext LastCommandContext;

		public CommandHandlingService(IServiceProvider services)
		{
			_commands = services.GetRequiredService<CommandService>();
			_discord = services.GetRequiredService<DiscordSocketClient>();
			_services = services;

			_discord.MessageReceived += MessageReceivedAsync;
		}

		public async Task InitializeAsync()
		{
			await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
		}

		public async Task MessageReceivedAsync(SocketMessage rawMessage)
		{
			if(rawMessage.Author.IsBot) return;
			Console.WriteLine("Message Recieved from " + rawMessage.Author.Username + ": " + rawMessage.Content);
			var message = rawMessage as SocketUserMessage;
			if (message == null) return;
			if (message.Content.StartsWith('!')) {
			

				var context = LastCommandContext = new SocketCommandContext(_discord, message);
				var result = await _commands.ExecuteAsync(context, 1, _services);
				if (!result.IsSuccess) await context.Channel.SendMessageAsync(result.ErrorReason);
			} else {
				Program.table.Add(message.Id, message.Author.Username, message.Content);
				Program.table.PrintTableNoPadding();
				FileInteraction.ConvertTableToFile(Program.table);
			}
		}
	}
}