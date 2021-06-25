using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Hitlady.Commands {
  public class AboutModule : BaseCommandModule {
    [Command("greet")]
    public async Task GreetCommand(CommandContext context) {
      await context.RespondAsync("Hello!");
    }
  }
}
