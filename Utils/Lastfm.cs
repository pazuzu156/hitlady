using Hqub.Lastfm;
using System.Threading.Tasks;

namespace Hitlady.Utils {
  public class LFM {
    private LastfmClient _client;

    private string m_LastfmUsername;

    public LFM(string lastfmUsername) {
      var fmConfig = Program.Config.Lastfm;
      m_LastfmUsername = lastfmUsername;

      _client = new LastfmClient(fmConfig.ApiKey);
    }

    public async Task<bool> UserExists() {
      bool exists = false;

      try {
        var user = await _client.User.GetInfoAsync(m_LastfmUsername);

        if (user.Name != null) {
          exists = true;
        }
      } catch {}

      return exists;
    }

    public async Task<Hqub.Lastfm.Entities.User> Artist(string name) {
      return await _client.User.GetInfoAsync(name);
    }
  }
}
