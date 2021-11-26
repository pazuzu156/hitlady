using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;

namespace Hitlady.Commands.Lastfm {
  [Group("plays")]
  public class PlaysModule : BaseFmModule {
    [GroupCommand]
    public async Task Default(CommandContext context) {
      var fm = await FM(context);
      var np = await fm.GetNowPlaying();

      if (np == null) {
        await context.RespondAsync("You're not currently listening to anything. You're not currently listening to anything.");
      }
    }

    [Command("artist"), Aliases("band")]
    public async Task Artist(CommandContext context, [RemainingText] string artist) {
      await context.RespondAsync("Artist");
    }
  }
}
