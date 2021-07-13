using System;
using System.Collections.Generic;
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

    private async Task Client_GuildMemberAdded(DiscordClient s, GuildMemberAddEventArgs e) => await sendGuildMemberMessage(s, MessageFilter.init(e.Guild, e.Member), _config.Messages.Join);

    private async Task Client_GuildMemberRemoved(DiscordClient s, GuildMemberRemoveEventArgs e) => await sendGuildMemberMessage(s, MessageFilter.init(e.Guild, e.Member), _config.Messages.Leave);

    private string getUser(DiscordMember member) => (!string.IsNullOrEmpty(member.Nickname)) ? member.Nickname : member.Username;

    private async Task sendGuildMemberMessage(DiscordClient client, MessageFilter filter, List<string> messages) {
      var rand = new Random();
      var index = getIndex(messages);

      if (_lastUsedIndex == 99) { // initial value, go ahead and change on new index
        _lastUsedIndex = index;
      }

      do {
        index = getIndex(messages);

        if (index != _lastUsedIndex) {
          _lastUsedIndex = index;

          break;
        }
      } while(index == _lastUsedIndex);

      await Program.Logger.Debug($"rand: {index}");
      var m = messages[index]; // get welcome message from our random number
      var result = filter.Replace(m, @"\%([A-Z_]+)\%");

      await Send(client, result);
    }

    private int getIndex(List<string> messages) {
      var rand = new Random();
      var index = rand.Next(0, messages.Count - 1); // generate a random int between 0 and number of welcome messages - 1

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
