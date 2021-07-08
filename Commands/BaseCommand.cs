using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using Hitlady.Data;
using Hitlady.Utils;

namespace Hitlady.Commands {
  public class BaseCommand : BaseCommandModule {
    protected ConfigYml _config;
    public BaseCommand() {
      _config = Settings.GetInstance().Config;
    }
    public void EmbedFooter(CommandContext context, in DiscordEmbedBuilder embedBuilder) {
      var user = context.Message.Author;
      embedBuilder.WithFooter($"Command invoked by: {user.Username}#{user.Discriminator}", user.AvatarUrl);
    }

    public async Task<DiscordMember> UserToMemberAsync(CommandContext context, DiscordUser user) {
      return await context.Guild.GetMemberAsync(user.Id);
    }

    public async Task<DiscordChannel> GetBotSpam(DiscordGuild guild) {
      // var channel = guild.GetChannel()
    }
  }
}
