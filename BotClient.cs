using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Hitlady.Data;
using Microsoft.Extensions.Logging;

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

    private Task Client_Ready(DiscordClient sender, ReadyEventArgs e)
    {
      _client.Logger.LogInformation("Bot is ready for use!");

      return Task.CompletedTask;
    }

    private async Task Client_GuildMemberAdded(DiscordClient s, GuildMemberAddEventArgs e) => await Send(s, $"{GetUser(e.Member)} has joined the server. Welcome!");

    private async Task Client_GuildMemberRemoved(DiscordClient s, GuildMemberRemoveEventArgs e) => await Send(s, $"{GetUser(e.Member)} left, maybe they'll be back?");

    private string GetUser(DiscordMember member) => (!string.IsNullOrEmpty(member.Nickname)) ? member.Nickname : member.Username;

    private async Task Send(DiscordClient client, string message) {
      var channel = await client.GetChannelAsync(_config.Channels.BotSpam);
      await channel.SendMessageAsync(message);
    }

    internal Task Commands_CommandExecuted(CommandsNextExtension sender, CommandExecutionEventArgs e)
    {
      e.Context.Client.Logger.LogInformation($"{e.Context.Member.Username}#{e.Context.Member.Discriminator} executed the '{e.Command.QualifiedName}' command successfully");

      return Task.CompletedTask;
    }
  }
}
