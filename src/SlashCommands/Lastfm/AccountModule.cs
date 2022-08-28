using Hitlady.Utils;
using ServiceStack.OrmLite;
using System;
using System.Threading.Tasks;
using DSharpPlus.SlashCommands;

namespace Hitlady.SlashCommands.Lastfm {
  public class AccountModule : SBaseModule {
    [SlashCommand("register", "Registers your LastFM esername")]
    public async Task RegisterCommand(InteractionContext context, [Option("username", "Your LastFM Username")] string username) {
      try {
        var db = await Data.Connection.Connect();
        var user = await db.SelectAsync<Data.User>(q => q.LastFM == username.Trim());

        var fm = new LFM(username);

        if (fm.UserExists().GetAwaiter().GetResult()) {
          if (user.Count == 0) {
            user = await db.SelectAsync<Data.User>(q => q.DiscordId == context.Member.Id);

            if (user.Count == 0) {
              var fmun = await fm.GetClient().User.GetInfoAsync(username.Trim());

              db.Insert(new Data.User{
                DiscordId = context.Member.Id,
                LastFM = fmun.Content.Name,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
              });

              await SendMessageAsync(context, "You've successfully registered your Last FM username");
            } else {
              await SendMessageAsync(context, $"You're already registered! Need to change your username? `{Program.Config.Prefix}unregister` first, then register again");
            }
          } else {
            await SendMessageAsync(context, "That LastFM username is already registered to another user!");
          }
        } else {
          await SendMessageAsync(context, "That username wasn't found on Lastfm!");
        }

        db.Close();
      } catch {
        await SendMessageAsync(context, "Couldn't register your username. Please try again later");
      }
    }

    [SlashCommand("unregister", "Unregisters your LastFM username")]
    public async Task UnregisterCommand(InteractionContext context) {
      try {
        var db = await Data.Connection.Connect();
        var exp = db.From<Data.User>().Where(q => q.DiscordId == context.Member.Id);
        var user = await db.SelectAsync(exp);

        if (user.Count > 0) {
          db.Delete(exp);
          await SendMessageAsync(context, "You have successfully unregistered your Last FM username");
        } else {
          await SendMessageAsync(context, "You don't have a Last FM username registered");
        }

        db.Close();
      } catch(Exception ex) {
        await SendMessageAsync(context, "There was an error unregistering you. Please try again later");
        Console.WriteLine(ex);
      }
    }
  }
}
