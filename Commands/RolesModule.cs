using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Hitlady.Commands
{
  [Group("roles"), Description("List server's joinable roles")]
  public class RolesModule : BaseCommand {
    [GroupCommand]
    public async Task ListRoles(CommandContext context) {
      var msg = "List of joinable roles:```";

      foreach (var role in context.Guild.Roles.Values) {
        if (IsRoleJoinable(role)) {
          if (HasRole(context.Member, role)) {
            msg += $"{role.Name.ToLower()} - You already have this role\n";
          } else {
            msg += $"{role.Name.ToLower()}\n";
          }
        }
      }

      await context.Message.RespondAsync($"{msg}```");
    }

    [Command("join"), Description("Join a role or list of roles")]
    public async Task Join(CommandContext context, [RemainingText, Description("Role(s) to join")] string roleString) {
      string[] rolesList = roleString.Split(new char[] { ',' });
      var joinedRoles = new List<DiscordRole>();

      foreach (var requestedRole in rolesList) {
        var role = GetRoleFromName(context.Guild, requestedRole.Trim());

        if (IsRoleJoinable(role)) {
          if (!HasRole(context.Member, role)) {
            await context.Member.GrantRoleAsync(role, $"{context.Member.Mention} has requested to join this role");
            joinedRoles.Add(role);
          } else {
            await context.RespondAsync($"{context.Member.Mention} you're already in the role {role.Name}");
          }
        } else {
          await context.RespondAsync($"{context.Member.Mention} the role {role.Name} is not joinable!");
        }
      }

      if (joinedRoles.Count == 1) {
        var roleName = joinedRoles[0].Name;
        await context.RespondAsync($"{context.Member.Mention} you have joined the role {roleName}");
      } else {
        string msg = "You have joined the roles: ";

        foreach (var role in joinedRoles) {
          msg += $"\"{role.Name}\", ";
        }

        await context.RespondAsync($"{context.Member.Mention} {msg.TrimEnd(new char[]{ ',', ' ' })}");
      }
    }

    [Command("leave"), Description("Leave a role or list of roles")]
    public async Task Leave(CommandContext context, [RemainingText, Description("Role(s) to leave")] string roleString) {
      string[] rolesList = roleString.Split(new char[] { ',' });
      var leftRoles = new List<DiscordRole>();

      foreach (var requestedRole in rolesList) {
        var role = GetRoleFromName(context.Guild, requestedRole.Trim());

        if (HasRole(context.Member, role)) {
            await context.Member.RevokeRoleAsync(role, $"{context.Member.Mention} has requested to leave this role");
            leftRoles.Add(role);
          } else {
            await context.RespondAsync($"{context.Member.Mention} you do not have the role {role.Name}");
          }
      }

      if (leftRoles.Count == 1) {
        var roleName = leftRoles[0].Name;
        await context.RespondAsync($"{context.Member.Mention} you have left the role {roleName}");
      } else {
        string msg = "You have left the roles: ";

        foreach (var role in leftRoles) {
          msg += $"\"{role.Name}\", ";
        }

        await context.RespondAsync($"{context.Member.Mention} {msg.TrimEnd(new char[]{ ',', ' ' })}");
      }
    }
  }
}
