using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using System.Threading.Tasks;
using DSharpPlus.SlashCommands;

namespace Hitlady.SlashCommands.Lastfm {
  [SlashCommandGroup("plays", "Returns play count information")]
  public class PlaysModule : BaseFmModule {
    [SlashCommand("track", "Gets current track play count")]
    public async Task Default(InteractionContext context) {
      var fm = await FM(context);
      var np = await fm.GetNowPlaying();

      if (np == null) {
        await SendMessageAsync(context, "You're not currently listening to anything. You're not currently listening to anything.");
      } else {
        var user = await GetDatabaseUser(context);
        await SendMessageAsync(context, $"**{user.LastFM}** has listened to *{np.Name}* by *{np.ArtistName}* **{np.UserPlayCount}** time(s).");
      }
    }

    [SlashCommand("artist", "Gets a play count of the requested artist")]
    public async Task Artist(InteractionContext context, [Option("artist", "The artist to search for. Leave empty for current playing artist")] string artist = "") {
      var fm = await FM(context);
      var user = await GetDatabaseUser(context);
      LastResponse<LastArtist> rArtist = null;

      if (artist == "") {
        var np = await fm.GetNowPlaying();
        rArtist = await fm.GetArtist(np.ArtistName);
      } else {
        rArtist = await fm.GetArtist(artist);
      }

      await SendMessageAsync(context, $"**{user.LastFM}** has listened to *{rArtist.Content.Name}* **{rArtist.Content.Stats.UserPlayCount}** time(s)");
    }

    [SlashCommand("album", "Gets a play count of the current playing album (Approximated play count)")]
    public async Task Album(InteractionContext context) {
      var fm = await FM(context);
      var np = await fm.GetNowPlaying();
      var user = await GetDatabaseUser(context);
      var rAlbum = await fm.GetAlbum(np.ArtistName, np.AlbumName);
      var playcount = rAlbum.Content.UserPlayCount / rAlbum.Content.Tracks.CountOrDefault();
      await SendMessageAsync(context, $"**{user.LastFM}** has listened through *{rAlbum.Content.Name}* by *{rAlbum.Content.ArtistName}* approximately **{playcount}** time(s)");
    }
  }
}
