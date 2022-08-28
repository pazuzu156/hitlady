using System;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;

namespace Hitlady.Commands
{
  [SlashCommandGroup("about", "Gets general bot info")] // this command is grouped (sub commands within about)
  public class AboutModule : BaseModule {
    private const string _inviteLink = ""; // I'll `maybe` use this at some point?

    private const string _sourceUrl = "https://github.com/pazuzu156/hitlady";

    [SlashCommand("bot", "Shows bot's info")] // this will be the base command method call
    public async Task AboutCommand(InteractionContext context) {
      await displayBotInfo(context);

      var prefix = _config.Prefix;
      var sb = new StringBuilder();
      var member = await UserToMemberAsync(context, context.Client.CurrentUser);
      sb.Append($"Want to find out more about {member.Username}?\n");
      sb.Append($"Type `{prefix}about uptime`, `{prefix}about source`");

      await SendMessageAsync(context, sb.ToString());
    }

    [SlashCommand("uptime", "Displays the amount of time the bot has been live"),]
    public async Task UptimeCommand(InteractionContext context) {
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
      await context.CreateResponseAsync(
        InteractionResponseType.ChannelMessageWithSource,
        new DiscordInteractionResponseBuilder().WithContent($"Uptime: {builder.ToString()}")
      );
    }

    [SlashCommand("source", "Gives the Github link to the source code")]
    public async Task SourceCommand(InteractionContext context)
      => await SendMessageAsync(context, _sourceUrl);

    private async Task displayBotInfo(InteractionContext context) {
      var member = await UserToMemberAsync(context, context.Client.CurrentUser);
      var rolesBuilder = new StringBuilder();

      foreach (var role in member.Roles) {
        rolesBuilder.Append($"{role.Name}, ");
      }

      var embed = new DiscordEmbedBuilder {
        Title = $"About {member.DisplayName}",
        Color = DiscordColor.Chartreuse,
        Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail {
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
      await context.CreateResponseAsync(
        InteractionResponseType.ChannelMessageWithSource,
        new DiscordInteractionResponseBuilder().AddEmbed(embed)
      );
    }
  }
}
