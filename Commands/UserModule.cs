using System;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Humanizer.DateTimeHumanizeStrategy;
using static DSharpPlus.Entities.DiscordEmbedBuilder;

namespace Hitlady.Commands {
  [Group("info"), Description("Gets user info")]
  public class UserModule : BaseCommand {
    [GroupCommand]
    public async Task GetUserInfo(CommandContext context, [RemainingText, Description("The user to get info on")] DiscordMember member = null) {
      if (member == null) {
        member = context.Member;
      }

      await context.RespondAsync(embed: buildUserInfoEmbed(member));
    }

    private DiscordEmbed buildUserInfoEmbed(DiscordMember member) {
      var embed = new DiscordEmbedBuilder {
        Color = DiscordColor.Teal,
        Thumbnail = new EmbedThumbnail {
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

      string presenceString = "";

      // try {
      //   presenceString = member.Presence.Activity.Name;
      // } catch {
      //   presenceString = "";
      // }

      try {
        var pres = member.Presence.Activity;
          Console.WriteLine(pres.RichPresence.Details);

        if (pres.RichPresence != null) {
          // rich presence
        } else {
          // regular
        }

        // if (pres.Name == "Custom Status") {
        //   embed.AddField("Status", member.Presence.Activity.CustomStatus.Name);
        // } else {
        //   if (pres.Name != "") {
        //     string fn = "Currently ";
        //   // embed.AddField("Currently Playing", presenceString);
        //     switch (pres.ActivityType) {
        //       case ActivityType.Playing:
        //         fn += "Playing";
        //         break;
        //       case ActivityType.ListeningTo:
        //         fn += "Listening to";
        //         break;
        //       case ActivityType.Watching:
        //         fn += "Watching";
        //         break;
        //       case ActivityType.Streaming:
        //         fn += "Streaming";
        //         break;
        //     }

        //     embed.AddField(fn, pres.Name);
        //   }
        // }
      } catch (Exception ex) {
        Console.WriteLine(ex);
      }

      // if (!presenceString.Equals("")) {
        // string fn = "Currently ";
        // // embed.AddField("Currently Playing", presenceString);
        // switch (member.Presence.Activity.ActivityType) {
        //   case ActivityType.Playing:
        //     fn += "Playing";
        //     break;
        //   case ActivityType.ListeningTo:
        //     fn += "Listening to";
        //     break;
        //   case ActivityType.Watching:
        //     fn += "Watching";
        //     break;
        //   case ActivityType.Streaming:
        //     fn += "Streaming";
        //     break;
        // }

      //   embed.AddField(fn, presenceString);
      // }



      return embed;
    }
  }
}
