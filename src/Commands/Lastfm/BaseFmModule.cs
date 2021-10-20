using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using Hitlady.Utils;

namespace Hitlady.Commands.Lastfm {
  public class BaseFmModule : BaseModule {
    protected async Task<LFM> FM(CommandContext context) {
      var user = await GetDatabaseUser(context);

      return new LFM(user.LastFM);
    }
  }
}
