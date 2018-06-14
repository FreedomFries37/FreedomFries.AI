using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RadDB3.interaction;
using RadDB3.structure;

namespace DiscordBot {
	public class Learning  : ModuleBase<SocketCommandContext> {

		[Command("database")]
		public Task DisplayWhatLearned() {
			string output = "TUPLES:\n";
			foreach (var radTuple in Program.table.All) {
				output += radTuple + "\n";
			}

			if (output == "") return Task.CompletedTask;
			return ReplyAsync(output);
		}

		[Command("addFromHere")]
		public async Task AddFromChannel() {
			SocketCommandContext last = CommandHandlingService.LastCommandContext;
			ISocketMessageChannel channel = last.Channel;

			var t = await channel.GetMessagesAsync(1000).Flatten().ToArray();
			foreach (IMessage message in t) {
				Program.table.Add(message.Id, message.Author.Username, message.Timestamp.Date, message.Content);
			}
			
			Program.table.PrintTableNoPadding();
			FileInteraction.ConvertDatabaseToFile(Program.db);
			Program.table.DumpData(25);
		}
	}
}