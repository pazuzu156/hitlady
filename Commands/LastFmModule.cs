using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using ServiceStack.OrmLite;
using System;
using System.Threading.Tasks;

namespace Hitlady.Commands {
  public class LastFmModule : BaseModule {
    [Command("register")]
    public async Task Register(CommandContext context, [RemainingText, Description("LastFM Username")] string username) {
      try {
        var db = await Sql.Connection.Connect();
        var user = await db.SelectAsync<Sql.User>(q => q.LastFM == username.Trim());

        if (user.Count == 0) {
          user = await db.SelectAsync<Sql.User>(q => q.DiscordId == context.Member.Id);

          if (user.Count == 0) {
            db.Insert(new Sql.User{
              DiscordId = context.Member.Id,
              LastFM = username.Trim(),
              CreatedAt = DateTime.UtcNow,
              UpdatedAt = DateTime.UtcNow
            });

            await context.RespondAsync("You've successfully registered your Last FM username");
          } else {
            await context.RespondAsync($"You're already registered! Need to change your username? `{Program.Config.Prefix}unregister` first, then register again");
          }
        } else {
          await context.RespondAsync("That LastFM username is already registered to another user!");
        }

        db.Close();
      } catch {
        await context.RespondAsync("Couldn't register your username. Please try again later");
      }
    }

    [Command("unregister")]
    public async Task Unregister(CommandContext context) {
      try {
        var db = await Sql.Connection.Connect();
        var exp = db.From<Sql.User>().Where(q => q.DiscordId == context.Member.Id);
        var user = await db.SelectAsync(exp);

        if (user.Count > 0) {
          db.Delete(exp);
          await context.RespondAsync("You have successfully unregistered your Last FM username");
        } else {
          await context.RespondAsync("You don't have a Last FM username registered");
        }
      } catch(Exception ex) {
        await context.RespondAsync("There was an error unregistering you. Please try again later");
        Console.WriteLine(ex);
      }
    }
  }
}
