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
    protected void EmbedFooter(CommandContext context, in DiscordEmbedBuilder embedBuilder) {
      var user = context.Message.Author;
      embedBuilder.WithFooter($"Command invoked by: {user.Username}#{user.Discriminator}", user.AvatarUrl);
    }

    protected async Task<DiscordMember> UserToMemberAsync(CommandContext context, DiscordUser user) {
      return await context.Guild.GetMemberAsync(user.Id);
    }

    protected DiscordChannel GetBotSpam(DiscordGuild guild) => guild.GetChannel(_config.Channels.BotSpam);

    /// <summary>
    /// Checks if a role is joinable or not
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    protected bool IsRoleJoinable(DiscordRole role)
    {
      bool isJoinable = false;

      foreach (var jr in _config.JoinableRoles)
      {
        if (role.Id == jr)
        {
          isJoinable = true;
        }
      }

      return isJoinable;
    }

    /// <summary>
    /// Checks if user has this role assigned.
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    protected bool HasRole(DiscordMember member, DiscordRole role) {
      foreach (var userRole in member.Roles) {
        if (role == userRole) {
          return true;
        }
      }

      return false;
    }

    /// <summary>
    /// Get a discord role from a string
    /// </summary>
    /// <param name="guild"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    protected DiscordRole GetRoleFromName(DiscordGuild guild, string name) {
      foreach (var role in guild.Roles.Values) {
        if (role.Name.ToLower() == name) {
          return role;
        }
      }

      return null;
    }
  }
}
