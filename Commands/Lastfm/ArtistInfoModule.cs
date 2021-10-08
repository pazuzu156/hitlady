using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using GScraper.Google;

namespace Hitlady.Commands.Lastfm {
  public class BandInfoModule : BaseFmModule {
    [Command("artistinfo"), Aliases("bi", "ai", "bandinfo", "band", "artist"), Description("Get information on a band/artist")]
    public async Task CommandGetArtistInfo(CommandContext context, [RemainingText, Description("The band to search info on")] string artist) {
      var user = await GetDatabaseUser(context);

      if (user != null) {
        if (artist == null) {
          var fm = await FM(context);
          var np = await fm.GetNowPlaying();

          if (np != null) {
            await ShowArtistInfo(context, np.ArtistName, true);
          } else {
            await context.RespondAsync("You're not currently listening to anything. Try searching with an artist name instead");
          }
        } else {
          await ShowArtistInfo(context, artist);
        }
      } else {
        if (artist != "") {
          await ShowArtistInfo(context, artist);
        } else {
          await context.RespondAsync("You're not registered to lastfm, so you need to supply an artist to search");
        }
      }
    }

    private async Task ShowArtistInfo(CommandContext context, string artist, bool withFmUser = false) {
      var fm = await FM(context);
      var fmArtist = await fm.GetArtist(artist);

      var scraper = new GoogleScraper();
      var images = await scraper.GetImagesAsync(fmArtist.Content.Name, type: GoogleImageType.Photo);
      var imageList = new List<string>();

      int x = 0, limit = 50;
      foreach (var image in images) {
        if (x < limit) {
          x++;
          imageList.Add(image.Url);
        } else {
          break; // break loop. no point scrapping 100 images with our limit (with this lib had a limit...)
        }
      }

      var embed = new DiscordEmbedBuilder {
        Title = fmArtist.Content.Name,
        Url = fmArtist.Content.Url.ToString(),
        Color = DiscordColor.Goldenrod,
        Timestamp = DateTime.UtcNow
      };

      var converter = new ReverseMarkdown.Converter();
      var bio = fmArtist.Content.Bio;
      var bioString = Truncate(converter.Convert(bio.Content), 850, $" [Read More...](https://last.fm/music/{fmArtist.Content.Name}/+wiki)");

      if (bio.Content == "") {
        bioString = "No bio found";
      }

      var tagsBuilder = new StringBuilder();
      var similarArtistsBuilder = new StringBuilder();

      foreach (var tag in fmArtist.Content.Tags) {
        tagsBuilder.Append($"[{tag.Name}]({tag.Url.ToString()}), ");
      }

      foreach (var s in fmArtist.Content.Similar) {
        similarArtistsBuilder.Append($"[{s.Name}]({s.Url}), ");
      }

      embed.AddField("Bio", bioString);
      embed.AddField("Tags", tagsBuilder.ToString().TrimEnd(new char[] {',', ' '}));
      embed.AddField("Similar Artists", similarArtistsBuilder.ToString().TrimEnd(new char[] {',', ' '}));
      embed.AddField("Total Listeners", string.Format("{0:n0}", fmArtist.Content.Stats.Listeners), true);

      if (imageList.Count > 0) {
        var rand = new Random();
        var index = rand.Next(0, imageList.Count - 1);
        embed.Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail {
          Url = imageList[index]
        };
      }

      EmbedFooter(context, in embed);
      await context.RespondAsync(embed: embed);
    }

    public string Truncate(string val, int max, string els = "...") {
      return val.Length <= max ? val : val.Substring(0, max) + els;
    }
  }
}
