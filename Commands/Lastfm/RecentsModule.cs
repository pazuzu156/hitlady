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
      var np = (List<string>)recents["nowplaying"];
      var fm = await FM(context);
      var album = await fm.GetAlbum(np[0], np[1]);

      // keep the following comment block, i'll  use this code later elsewhere
      // attempt to scrape image
      // var scraper = new GoogleScraper();
      // var images = await scraper.GetImagesAsync($"{np[0]} {np[1]}", type: GoogleImageType.Photo);
      // var imageList = new List<string>();

      // int x = 0;
      // foreach (var image in images) {
      //   if (x < 10) {
      //     x++;
      //     imageList.Add(image.Url);
      //   } else {
      //     break; // break loop. no point scrapping 100 images with our limit (with this lib had a limit...)
      //   }
      // }

      // var rand = new Random();
      // var index = rand.Next(0, imageList.Count - 1);

      var embed = new DiscordEmbedBuilder {
        Title = $"Now Playing for {context.User.Username}",
        Color = DiscordColor.IndianRed,
        Timestamp = DateTime.UtcNow,
        Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail {
          Url = album.Content.Images.Largest.ToString()
        }
      };

      embed.AddField("Currently Playing", $"{np[0]} {np[2]}");
      embed.AddField("Recent Plays", (string)recents["recents"]);

      EmbedFooter(context, in embed);
      await context.RespondAsync(embed: embed);
    }

    [Command("recent"), Description("Gets a list of recent plays")]
    public async Task RecentPlaysCommand(CommandContext context) {
      var recents = await GetRecents(context);

      var embed = new DiscordEmbedBuilder {
        Title = $"Recent Plays for {context.User.Username}",
        Color = DiscordColor.IndianRed,
        Description = (string)recents["recents"],
        Timestamp = DateTime.UtcNow
      };

      EmbedFooter(context, in embed);
      await context.RespondAsync(embed: embed);
    }

    protected async Task<Dictionary<string, object>> GetRecents(CommandContext context) {
      var fm = await FM(context);
      var recentTracks = await fm.GetRecentTracks();
      var sb = new StringBuilder();
      var x = 1;
      var list = new Dictionary<string, object>();

      foreach (var track in recentTracks) {
        if (track.IsNowPlaying == null) {
          sb.Append($"{x}: {track.ArtistName} - {track.Name}\n");
          x++;
        } else {
          var album = await fm.GetAlbum(track.ArtistName, track.AlbumName);
          list.Add("nowplaying", new List<string>() {
            track.ArtistName,
            track.AlbumName,
            track.Name
          });
        }
      }

      list.Add("recents", sb.ToString());

      return list;
    }
  }
}
