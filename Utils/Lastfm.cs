using System.Threading.Tasks;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Objects;

namespace Hitlady.Utils {
  public class LFM {
    private LastfmClient _client;

    private string m_LastfmUsername;

    public LFM(string lastfmUsername) {
      var fmConfig = Program.Config.Lastfm;
      m_LastfmUsername = lastfmUsername;

      _client = new LastfmClient(fmConfig.ApiKey, fmConfig.ApiSecret);
    }

    public async Task<bool> UserExists() {
      bool exists = false;

      try {
        var user = await _client.User.GetInfoAsync(m_LastfmUsername);

        if (user.Content.Name != null) {
          exists = true;
        }
      } catch {}

      return exists;
    }

    public async Task<LastTrack> GetNowPlaying() {
      var tracks = await _client.User.GetRecentScrobbles(m_LastfmUsername, count: 3);

      foreach (var track in tracks) {
        if ((bool)track.IsNowPlaying) {
          return track;
        }
      }

      return null;
    }
  }
}
