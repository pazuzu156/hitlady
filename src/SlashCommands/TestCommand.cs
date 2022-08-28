using System.Threading.Tasks;
using DSharpPlus.SlashCommands;

namespace Hitlady.SlashCommands {
  public class TestCommand : SBaseModule {
    [SlashCommand("test", "A test slash command")]
    public async Task TC(InteractionContext ctx, [Option("name", "Your name")] string name = "") {
      string message = "Hello";

      if (name != "") {
        message += $" {name}";
      }
      await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DSharpPlus.Entities.DiscordInteractionResponseBuilder().WithContent(message+"!"));
    }
  }
}
