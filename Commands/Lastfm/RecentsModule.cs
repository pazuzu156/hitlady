using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Hitlady.Commands.Lastfm {
  public class NowPlayingModule : BaseFmModule {
    [Command("nowplaying"), Aliases("np"), Description("Gets the current playing tack")]
    public async Task NowPlayingCommand(CommandContext context) {
      var recents = await GetRecents(context);

      var embed = new DiscordEmbedBuilder {
        Title = $"Now Playing for {context.User.Username}",
        Color = DiscordColor.IndianRed,
        Timestamp = DateTime.UtcNow
      };

      embed.AddField("Currently Playing", recents[0]);
      embed.AddField("Recent Plays", recents[1]);

      EmbedFooter(context, in embed);
      await context.RespondAsync(embed: embed);
    }

    [Command("recent"), Description("Gets a list of recent plays")]
    public async Task RecentPlaysCommand(CommandContext context) {
      var recents = await GetRecents(context);

      var embed = new DiscordEmbedBuilder {
        Title = $"Recent Plays for {context.User.Username}",
        Color = DiscordColor.IndianRed,
        Description = recents[1],
        Timestamp = DateTime.UtcNow
      };

      EmbedFooter(context, in embed);
      await context.RespondAsync(embed: embed);
    }

    protected async Task<List<string>> GetRecents(CommandContext context) {
      var fm = await FM(context);
      var recentTracks = await fm.GetRecentTracks();
      var sb = new StringBuilder();
      var x = 1;
      var list = new List<string>();

      foreach (var track in recentTracks) {
        if (track.IsNowPlaying == null) {
          sb.Append($"{x}: {track.ArtistName} - {track.Name}\n");
          x++;
        } else {
          list.Add($"{track.ArtistName} - {track.Name}");
        }
      }

      list.Add(sb.ToString());

      return list;
    }
  }
}
