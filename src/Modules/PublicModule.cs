using System.Threading.Tasks;
using Discord.Commands;
using Discord;

namespace DiscordBot {
	public class PublicModule : ModuleBase<SocketCommandContext> {


		[Command("Your mom")]
		[Alias("ur mum")]
		public Task PingYourMom() => ReplyAsync("YOUR MOM!");
	}
}