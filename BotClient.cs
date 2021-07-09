using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Hitlady.Data;
using Hitlady.Utils;

namespace Hitlady {
  class BotClient {
    private ConfigYml _config;

    private DiscordClient _client;

    private int _lastUsedIndex = 99;
    public BotClient(DiscordClient client) {
      _config = Program.Config;
      _client = client;

      client.Ready += Client_Ready;
      client.GuildMemberAdded += Client_GuildMemberAdded;
      client.GuildMemberRemoved += Client_GuildMemberRemoved;
    }

    private async Task Client_Ready(DiscordClient sender, ReadyEventArgs e) => await Program.Logger.Info("Bot is ready for use!");

    private async Task Client_GuildMemberAdded(DiscordClient s, GuildMemberAddEventArgs e) {
      var filter = MessageFilter.init(e.Guild, e.Member);
      var rand = new Random();
      var index = getIndex();

      if (_lastUsedIndex == 99) { // initial value, go ahead and change on new index
        _lastUsedIndex = index;
      }

      do {
        index = getIndex();

        if (index != _lastUsedIndex) {
          _lastUsedIndex = index;

          break;
        }
      } while(index == _lastUsedIndex);

      await Program.Logger.Debug($"rand: {index}");
      var welcomeMessage = _config.WelcomeMessages[index]; // get welcome message from our random number
      var result = filter.Replace(welcomeMessage, @"\%([A-Z_]+)\%");

      await Send(s, result);
    }

    private async Task Client_GuildMemberRemoved(DiscordClient s, GuildMemberRemoveEventArgs e) => await Send(s, $"{getUser(e.Member)} left.");

    private string getUser(DiscordMember member) => (!string.IsNullOrEmpty(member.Nickname)) ? member.Nickname : member.Username;

    private int getIndex() {
      var rand = new Random();
      var index = rand.Next(0, _config.WelcomeMessages.Count - 1); // generate a random int between 0 and number of welcome messages - 1

      return index;
    }

    private async Task Send(DiscordClient client, string message) {
      var channel = await client.GetChannelAsync(_config.Channels.BotSpam);
      await channel.SendMessageAsync(message);
    }

    internal async Task Commands_CommandExecuted(CommandsNextExtension sender, CommandExecutionEventArgs e)
      => await Program.Logger.Info($"{e.Context.Member.Username}#{e.Context.Member.Discriminator} executed the '{e.Command.QualifiedName}' command successfully");
  }
}
