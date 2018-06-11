
using System;
using System.ComponentModel.Design;
using System.Net.Http;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using RadDB3.interaction;
using RadDB3.structure;
using RadDB3.structure.Types;


namespace DiscordBot {

	
	class Program {
		private const string TOKEN = "NDU1ODA0MDA0MzMzNjQ5OTUy.DgBbvw.WqVs0XFqoCKWeRLo8utYh57Bg5A";
		
		static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();
		private DiscordSocketClient client;
		public static Table table;
		private static readonly Relation Relation = new Relation(("*ID", typeof(RADuLong)), ("Name", typeof(RADString)), ("Message", typeof(RADString)));
	
		public async Task MainAsync() {
			var services = ConfigureServices();

			client = services.GetRequiredService<DiscordSocketClient>();

			client.Log += LogAsync;
			client.Ready += ReadyAsync;
			services.GetRequiredService<CommandService>().Log += LogAsync;
			await client.LoginAsync(TokenType.Bot, TOKEN);
			await client.StartAsync();

			await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

			table = new Table(Relation);
			
			await Task.Delay(-1);
		}

		private Task LogAsync(LogMessage log) {
			Console.WriteLine(log.ToString());
			return Task.CompletedTask;
		}

		private Task ReadyAsync() {
			Console.WriteLine($"{client.CurrentUser} is connected!");
			return Task.CompletedTask;
		}

		private IServiceProvider ConfigureServices() {
			return new ServiceCollection()
				.AddSingleton<DiscordSocketClient>()
				.AddSingleton<CommandService>()
				.AddSingleton<CommandHandlingService>()
				.AddSingleton<HttpClient>()
				.BuildServiceProvider();
		}

	}
}