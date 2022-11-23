using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Hitlady.Commands.Lastfm {
  public class TasteModule : BaseFmModule {
    [SlashCommand("taste", "Stack your musical tastes up with others")]
    public async Task TaseCommand(InteractionContext context, [Option("user", "A user to get compaire tastes with")] DiscordUser user) {
      // if (user.Id == context.User.Id) {
      //   await SendMessageAsync(context, "You must taste good if you want to taste yourself");
      // }
      var taster = await FM(context);
      var tu = await taster.GetUser();
      var victim = await FM(user);
      var vu = await victim.GetUser();

      var tasterData = await taster.GetTopArtists(5);
      // var victimData = await victim.GetTopArtists(700);
      var sb = new StringBuilder();

      // foreach (var x in tasterData) {
      //   sb.Append($"Artist: {x}\n");
      // }

      // await SendMessageAsync(context, tasterData.TotalItems+"");
    }
  }
}
