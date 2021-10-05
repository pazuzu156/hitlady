using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hitlady.Commands.Lastfm {
  public class YouTubeModule : BaseModule {
    private string m_RootUrl = "https://www.googleapis.com/youtube/v3/search";

    [Command("youtube"), Aliases("yt")]
    public async Task Yt(CommandContext context, [RemainingText]string item) {
      if (item != "") {
        await Search(context, item);
      } else {
        // get np

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

      await context.RespondAsync($"Result for **{item}**: https://youtu.be/{videos[0].Id.VideoId}");
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
