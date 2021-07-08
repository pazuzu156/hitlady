using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Hitlady.Data;

namespace Hitlady {
  class BotClient {
    private ConfigYml _config;

    private DiscordClient _client;
    public BotClient(DiscordClient client) {
      _config = Program.Config;
      _client = client;

      client.Ready += Client_Ready;
      client.GuildMemberAdded += Client_GuildMemberAdded;
      client.GuildMemberRemoved += Client_GuildMemberRemoved;
    }

    private async Task Client_Ready(DiscordClient sender, ReadyEventArgs e) => await Program.Logger.Info("Bot is ready for use!");

    private async Task Client_GuildMemberAdded(DiscordClient s, GuildMemberAddEventArgs e) => await Send(s, $"{GetUser(e.Member)} has joined the server. Welcome!");

    private async Task Client_GuildMemberRemoved(DiscordClient s, GuildMemberRemoveEventArgs e) => await Send(s, $"{GetUser(e.Member)} left, maybe they'll be back?");

    private string GetUser(DiscordMember member) => (!string.IsNullOrEmpty(member.Nickname)) ? member.Nickname : member.Username;

    private async Task Send(DiscordClient client, string message) {
      var channel = await client.GetChannelAsync(_config.Channels.BotSpam);
      await channel.SendMessageAsync(message);
    }

    internal async Task Commands_CommandExecuted(CommandsNextExtension sender, CommandExecutionEventArgs e)
      => await Program.Logger.Info($"{e.Context.Member.Username}#{e.Context.Member.Discriminator} executed the '{e.Command.QualifiedName}' command successfully");
  }
}
