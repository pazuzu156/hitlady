using System.Data;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Hitlady.Data;
using Hitlady.Utils;

namespace Hitlady.SlashCommands {
  public class SBaseModule : ApplicationCommandModule {
    protected ConfigYml _config;

    public SBaseModule() {
      _config = Settings.GetInstance().Config;
    }

    protected void EmbedFooter(InteractionContext ctx, in DiscordEmbedBuilder embed) {
      embed.WithFooter($"Command invoked by: {ctx.User.Username}#{ctx.User.Discriminator}", ctx.User.AvatarUrl);
    }

    protected async Task<IDbConnection> Db() {
      return await Connection.Connect();
    }

    protected async Task<DiscordMember> UserToMemberAsync(InteractionContext ctx, DiscordUser user) {
      return await ctx.Guild.GetMemberAsync(user.Id);
    }
  }
}
