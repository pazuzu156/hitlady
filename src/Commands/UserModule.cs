using System;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Humanizer.DateTimeHumanizeStrategy;

namespace Hitlady.Commands {
  [Group("info"), Description("Gets user info")]
  public class UserModule : BaseModule {
    [GroupCommand]
    public async Task GetUserInfoCommand(CommandContext context, [RemainingText, Description("The user to get info on")] DiscordMember member = null) {
      if (member == null) {
        member = context.Member;
      }

      await context.RespondAsync(embed: buildUserInfoEmbed(member));
    }

    private DiscordEmbed buildUserInfoEmbed(DiscordMember member) {
      var embed = new DiscordEmbedBuilder {
        Color = DiscordColor.Teal,
        Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail {
          Url = member.AvatarUrl
        },
        Timestamp = DateTime.UtcNow
      };

      string name = string.Format(
        "{0}#{1}{2}",
        member.DisplayName,
        member.Discriminator,
        (member.IsBot) ? " (Bot)" : ""
      );

      embed.AddField("ID", member.Id.ToString());
      embed.AddField("Name", name);

      if (member.Nickname != null) {
        embed.AddField("Nickname", member.Nickname);
      }

      DefaultDateTimeHumanizeStrategy s = new DefaultDateTimeHumanizeStrategy();
      var joined = member.JoinedAt.UtcDateTime;
      var h = s.Humanize(joined, DateTime.UtcNow, CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"));
      string joinedString = string.Format(
        "{0} {1} ({2})",
        joined.ToLongDateString(),
        joined.ToLongTimeString(),
        h
      );
      embed.AddField("Joined Server", joinedString);

      var rolesBuilder = new StringBuilder();

      foreach (var role in member.Roles) {
        rolesBuilder.Append(role.Name + ", ");
      }

      var rolesString = rolesBuilder.ToString().TrimEnd(new char[] { ',', ' ' });

      if (rolesString.Equals("")) {
        rolesString = "No roles assigned";
      }

      embed.AddField("Roles", rolesString);

      return embed;
    }
  }
}
