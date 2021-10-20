using System.Threading.Tasks;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Helpers;
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

    public LastfmClient GetClient() {
      return _client;
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
      var tracks = await GetRecentTracks(3);

      foreach (var track in tracks) {
        if (track.IsNowPlaying != null) {
          return track;
        }
      }

      return null;
    }

    public async Task<PageResponse<LastTrack>> GetRecentTracks(int limit = 5) {
      return await _client.User.GetRecentScrobbles(m_LastfmUsername, count: limit);
    }

    public async Task<LastResponse<LastAlbum>> GetAlbum(string artist, string album) {
      return await _client.Album.GetInfoAsync(artist, album);
    }

    public async Task<LastResponse<LastArtist>> GetArtist(string artist) {
      return await _client.Artist.GetInfoAsync(artist);
    }
  }
}
