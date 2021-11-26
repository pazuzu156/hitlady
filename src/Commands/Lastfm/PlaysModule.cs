using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using System.Threading.Tasks;

namespace Hitlady.Commands.Lastfm {
  [Group("plays")]
  public class PlaysModule : BaseFmModule {
    [GroupCommand, Aliases("track")]
    public async Task Default(CommandContext context) {
      var fm = await FM(context);
      var np = await fm.GetNowPlaying();

      if (np == null) {
        await context.RespondAsync("You're not currently listening to anything. You're not currently listening to anything.");
      } else {
        var user = await GetDatabaseUser(context);
        await context.RespondAsync($"**{user.LastFM}** has listened to *{np.Name}* by *{np.ArtistName}* **{np.UserPlayCount}** time(s).");
      }
    }

    [Command("artist"), Aliases("band", "group")]
    public async Task Artist(CommandContext context, [RemainingText] string artist) {
      var fm = await FM(context);
      var user = await GetDatabaseUser(context);
      LastResponse<LastArtist> rArtist = null;

      if (artist == null) {
        var np = await fm.GetNowPlaying();
        rArtist = await fm.GetArtist(np.ArtistName);
      } else {
        rArtist = await fm.GetArtist(artist);
      }

      await context.RespondAsync($"**{user.LastFM}** has listened to *{rArtist.Content.Name}* **{rArtist.Content.Stats.UserPlayCount}** time(s)");
    }

    [Command("album")]
    public async Task Album(CommandContext context) {
      var fm = await FM(context);
      var np = await fm.GetNowPlaying();
      var user = await GetDatabaseUser(context);
      var rAlbum = await fm.GetAlbum(np.ArtistName, np.AlbumName);
      var playcount = rAlbum.Content.UserPlayCount / rAlbum.Content.Tracks.CountOrDefault();
      await context.RespondAsync($"**{user.LastFM}** has listened through *{rAlbum.Content.Name}* by *{rAlbum.Content.ArtistName}* approximately **{playcount}** time(s)");
    }
  }
}
