using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Hitlady.Utils;

namespace Hitlady.Commands.Lastfm {
  public class BaseFmModule : BaseModule {
    protected async Task<LFM> FM(InteractionContext context) {
      var user = await GetDatabaseUser(context);

      return new LFM(user.LastFM);
    }

    protected async Task<LFM> FM(DiscordUser user)
      => new LFM(((Data.User)await GetDatabaseUser(user)).LastFM);

    protected async Task<LFM> FM(DiscordMember member)
      => new LFM(((Data.User)await GetDatabaseUser(member)).LastFM);
  }
}
