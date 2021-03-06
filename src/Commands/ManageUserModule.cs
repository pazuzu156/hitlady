using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Hitlady.Commands {
  public class ManageUserModule : BaseModule {
    [Command("kick"), RequirePermissions(DSharpPlus.Permissions.KickMembers), Description("Kicks a user [Requires KickMembers persission]")]
    public async Task KickUserCommand(CommandContext context, [Description("The member to kick")] DiscordMember member, [RemainingText, Description("The reason for kicking the user")] string reason = "") {
      reason = reason.Trim(new char[0]);

      if (reason == "") {
        reason = "None Given";
      }

      var kicker = context.Member.DisplayName;
      var msg = $"{kicker} kicked you. | Reason: {reason}";
      var embed = new DiscordEmbedBuilder {
        Title = $"User {member.DisplayName} was kicked",
        Color = DiscordColor.Goldenrod
      };
      embed.AddField("Kicked by", kicker, true);
      embed.AddField("Reason", reason, true);

      await member.SendMessageAsync(msg);
      await member.RemoveAsync(reason);
      await GetBotSpam(context.Guild).SendMessageAsync(embed: embed);
    }

    [Command("ban"), RequirePermissions(DSharpPlus.Permissions.BanMembers), Description("Bans a user [Requires BanMembers persission]")]
    public async Task BanUserCommand(CommandContext context, [Description("The member to ban")] DiscordMember member, [RemainingText, Description("The reason for banning the user")] string reason = "") {
      reason = reason.Trim(new char[0]);

      if (reason == "") {
        reason = "None Given";
      }

      var kicker = context.Member.DisplayName;
      var msg = $"{kicker} banned you. | Reason: {reason}";
      var embed = new DiscordEmbedBuilder {
        Title = $"User {member.DisplayName} was banned",
        Color = DiscordColor.Goldenrod
      };
      embed.AddField("Banned by", kicker, true);
      embed.AddField("Reason", reason, true);

      await member.SendMessageAsync(msg);
      await member.BanAsync(7, reason);
      await GetBotSpam(context.Guild).SendMessageAsync(embed: embed);
    }
  }
}
