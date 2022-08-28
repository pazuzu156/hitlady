using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;
using DSharpPlus.SlashCommands;

namespace Hitlady.Commands.Lastfm {
  public class YouTubeModule : BaseFmModule {
    private string m_RootUrl = "https://www.googleapis.com/youtube/v3/search";

    [SlashCommand("youtube", "Search for a Youtube video from either the current playing tack, or a custom query")]
    public async Task YtCommand(InteractionContext context, [Option("query", "YouTube Search Query (Empty for current track)")]string item = "") {
      if (item != "") {
        await Search(context, item);
      } else {
        var fm = await FM(context);
        var np = await fm.GetNowPlaying();

        if (np == null) {
          await SendMessageAsync(context, "You're not currently listening to anything. Try searching for something instead.");
        } else {
          await Search(context, $"{np.ArtistName} {np.Name}");
        }
      }
    }

    /// <summary>
    /// Performs a search for an item against the YouTube API.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public async Task Search(InteractionContext context, string item) {
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

      await SendMessageAsync(context, $"YouTube search result for **{item}**:\nhttps://youtu.be/{videos[0].Id.VideoId}");
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
