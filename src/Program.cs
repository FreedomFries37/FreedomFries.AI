
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
		private const string TOKEN = "";
		
		static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();
		private DiscordSocketClient client;
		public static Database db;
		private static readonly Relation Relation = new Relation(
			("*ID", typeof(RADuLong)),
			("Name", typeof(RADString)),
			("&Time", typeof(RADDateTime)),
			("Message", typeof(RADString)));

		public static Table table => db["IDNTM"];
	
		public async Task MainAsync() {
			var services = ConfigureServices();

			client = services.GetRequiredService<DiscordSocketClient>();

			client.Log += LogAsync;
			client.Ready += ReadyAsync;
			services.GetRequiredService<CommandService>().Log += LogAsync;
			await client.LoginAsync(TokenType.Bot, TOKEN);
			await client.StartAsync();

			await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

			if (FileInteraction.ConvertDirectoriesInCurrentDirectoryToDatabases().Length == 0) {
				db = new Database("Discord_Database", 5);
				db.addTable(new Table("IDNTM", Relation));
				FileInteraction.ConvertDatabaseToFile(db);
			} else {
				db = FileInteraction.ConvertDirectoriesInCurrentDirectoryToDatabases()[0];
			}
			
			
			
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