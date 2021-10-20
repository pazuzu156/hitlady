using System;
using ServiceStack.DataAnnotations;

namespace Hitlady.Data {
  [Alias("users")]
  public class User {
    [PrimaryKey, AutoIncrement, Alias("id")]
    public int Id { get; set; }

    [Alias("discord_id"), Unique]
    public ulong DiscordId { get; set; }

    [Alias("lastfm")]
    public string LastFM { get; set; }

    [Alias("created_at")]
    public DateTime CreatedAt { get; set; }

    [Alias("updated_at")]
    public DateTime UpdatedAt { get; set; }
  }
}
