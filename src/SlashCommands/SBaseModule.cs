using System.Data;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Hitlady.Data;
using Hitlady.Utils;
using ServiceStack.OrmLite;

namespace Hitlady.SlashCommands {
  public class SBaseModule : ApplicationCommandModule {
    protected ConfigYml _config;
    public SBaseModule() {
      _config = Settings.GetInstance().Config;
    }

    /// <summary>
    /// Generates a footer for use on embeds.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="embedBuilder"></param>
    protected void EmbedFooter(InteractionContext context, in DiscordEmbedBuilder embedBuilder)
      => embedBuilder.WithFooter($"Command invoked by: {context.Member.Username}#{context.Member.Discriminator}", context.Member.AvatarUrl);

    /// <summary>
    /// Converts a DiscordUser to a DiscordMember.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    protected async Task<DiscordMember> UserToMemberAsync(InteractionContext context, DiscordUser user)
      => await context.Guild.GetMemberAsync(user.Id);

    protected DiscordChannel GetBotSpam(DiscordGuild guild)
      => guild.GetChannel(_config.Channels.BotSpam);

    /// <summary>
    /// Checks if a role is joinable or not.
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    protected bool IsRoleJoinable(DiscordRole role) {
      bool isJoinable = false;

      foreach (var jr in _config.JoinableRoles) {
        if (role.Id == jr) {
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
    /// Get a discord role from a string.
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

    /// <summary>
    /// Gets a user from the database.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    protected async Task<Data.User> GetDatabaseUser(InteractionContext context) {
      var db = await Db();
      var user = await db.SelectAsync<Data.User>(q => q.DiscordId == context.User.Id);
      db.Close();

      if (user.Count > 0) {
        return user[0];
      }

      return null;
    }

    /// <summary>
    /// Sends a string message.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    protected async Task SendMessageAsync(InteractionContext context, string message) {
      await context.CreateResponseAsync(
        DSharpPlus.InteractionResponseType.ChannelMessageWithSource,
        new () { Content = message }
      );
    }

    /// <summary>
    /// Sends an embed message.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="embed"></param>
    /// <returns></returns>
    protected async Task SendMessageAsync(InteractionContext context, DiscordEmbed embed) {
      await context.CreateResponseAsync(
        DSharpPlus.InteractionResponseType.ChannelMessageWithSource,
        new DiscordInteractionResponseBuilder().AddEmbed(embed)
      );
    }

    protected async Task<IDbConnection> Db() {
      return await Connection.Connect();
    }
  }
}
