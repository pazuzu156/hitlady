using System;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Hitlady.Utils;
using static DSharpPlus.Entities.DiscordEmbedBuilder;

namespace Hitlady.Commands
{
  [Group("about"), Description("Shows info about the bot")] // this command is grouped (sub commands within about)
  public class AboutModule : BaseModule {
    private const string _inviteLink = ""; // I'll `maybe` use this at some point?

    private const string _sourceUrl = "https://gitlab.com/pazuzu156/hitlady";

    [GroupCommand] // this will be the base command method call
    public async Task AboutCommand(CommandContext context) {
      await displayBotInfo(context);

      var prefix = _config.Prefix;
      var sb = new StringBuilder();
      var member = await UserToMemberAsync(context, context.Client.CurrentUser);
      sb.Append($"Want to find out more about {member.Username}?\n");
      sb.Append($"Type `{prefix}about uptime`, `{prefix}about source`");

      await context.RespondAsync(sb.ToString());
    }

    [Command("uptime"), Description("Displays the amount of time the bot has been live")]
    public async Task Uptime(CommandContext context) {
      var delta = DateTime.UtcNow - Program.StartTime;
      var days = delta.Days.ToString("n0");
      var hrs = delta.Hours.ToString("n0");
      var mins = delta.Minutes.ToString("n0");
      var secs = delta.Seconds.ToString("n0");

      var builder = new StringBuilder();

      if (!days.Equals("0")) {
        builder.Append($"{days} days ");
      }

      if (!hrs.Equals("0")) {
        builder.Append($"{hrs} hours ");
      }

      if (!mins.Equals("0")) {
        builder.Append($"{mins} minutes ");
      }

      builder.Append($"{secs} seconds ");
      await context.RespondAsync($"Uptime: {builder.ToString()}");
    }

    [Command("source"), Description("Gives the Github link to the source code")]
    public async Task Source(CommandContext context) {
      await context.RespondAsync($"{_sourceUrl}");
    }

    private async Task displayBotInfo(CommandContext context) {
      var member = await UserToMemberAsync(context, context.Client.CurrentUser);
      var rolesBuilder = new StringBuilder();

      foreach (var role in member.Roles) {
        rolesBuilder.Append($"{role.Name}, ");
      }

      var embed = new DiscordEmbedBuilder {
        Title = $"About {member.DisplayName}",
        Color = DiscordColor.Chartreuse,
        Thumbnail = new EmbedThumbnail {
          Url = member.AvatarUrl
        },
        Description = $"{member.DisplayName} is a bot written in C#. Version: {Program.Version.ToString()}",
        Timestamp = DateTime.UtcNow,
      };

      embed.AddField("Name", $"{member.DisplayName}#{member.Discriminator}");
      embed.AddField("ID", member.Id.ToString());

      if (member.Nickname != null) {
        embed.AddField("Nickname", member.Nickname);
      }

      embed.AddField("Roles", rolesBuilder.ToString().TrimEnd(new char[] {',', ' '}));

      EmbedFooter(context, in embed);
      await context.RespondAsync(embed: embed);
    }
  }
}
