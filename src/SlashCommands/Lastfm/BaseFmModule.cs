using System.Threading.Tasks;
using DSharpPlus.SlashCommands;
using Hitlady.Utils;

namespace Hitlady.SlashCommands.Lastfm {
  public class BaseFmModule : SBaseModule {
    protected async Task<LFM> FM(InteractionContext context) {
      var user = await GetDatabaseUser(context);

      return new LFM(user.LastFM);
    }
  }
}
