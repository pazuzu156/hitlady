using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Newtonsoft.Json.Linq;

namespace Hitlady.Commands.Lastfm {
  public class YouTubeModule : BaseFmModule {
    private string m_RootUrl = "https://www.googleapis.com/youtube/v3/search";

    [Command("youtube"), Aliases("yt"), Description("Search for a Youtube video from either the current playing tack, or a custom query")]
    public async Task YtCommand(CommandContext context, [RemainingText, Description("Search query")]string item) {
      if (item != null) {
        await Search(context, item);
      } else {
        var fm = await FM(context);
        var np = await fm.GetNowPlaying();

        if (np == null) {
          await context.RespondAsync("You're not currently listening to anything. Try searching for something instead.");
        } else {
          await Search(context, $"{np.ArtistName} {np.Name}");
        }
      }
    }

    public async Task Search(CommandContext context, string item) {
      var client = new HttpClient();
      NameValueCollection nvc = new NameValueCollection(){
        {"key", _config.YouTube.ApiKey},
        {"part", "snippet"},
        {"type", "video"},
        {"q", item}
      };

      var queryString = $"key={_config.YouTube.ApiKey}&part=snippet&type=video&q=" + HttpUtility.UrlEncode(item);
      var response = await client.GetAsync($"{m_RootUrl}?{queryString}");

      var data = JObject.Parse(await response.Content.ReadAsStringAsync());
      IList<JToken> res = data["items"].Children().ToList();
      IList<VideoItems> videos = new List<VideoItems>();

      foreach (JToken r in res) {
        VideoItems i = r.ToObject<VideoItems>();
        videos.Add(i);
      }

      await context.RespondAsync($"YouTube search result for **{item}**:\nhttps://youtu.be/{videos[0].Id.VideoId}");
    }

    public class VideoItems {
      public struct SID {
        public string Kind { get; set; }
        public string VideoId { get; set; }
      }

      public SID Id { get; set; }
    }
  }
}
