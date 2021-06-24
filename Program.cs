﻿using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;

using hitlady.Utils;

namespace hitlady {
  class Program {
    private Settings _settings;
    static void Main(string[] args) {
      new Program();
    }

    public Program() {
      _settings = Settings.GetInstance();

      MainAsync().GetAwaiter().GetResult();
    }

    private async Task MainAsync() {
      var discord = new DiscordClient(new DiscordConfiguration() {
        Token = _settings.Config.Token,
        TokenType = TokenType.Bot,
        Intents = DiscordIntents.AllUnprivileged | DiscordIntents.GuildMembers
      });

      discord.Ready += (s, e) => {
        Console.WriteLine("Bot is ready for use!");

        return Task.CompletedTask;
      };

      discord.GuildMemberAdded += async (s, e) => {
        await Send(s, $"{GetUser(e.Member)} has joined the server. Welcome!");
      };

      discord.GuildMemberRemoved += async (s, e) => {
        await Send(s, $"{GetUser(e.Member)} left, maybe they'll be back?");
      };

      await discord.ConnectAsync();
      await Task.Delay(-1);
    }

    private string GetUser(DiscordMember member) {
      var name = member.Nickname;

      return (!string.IsNullOrEmpty(name)) ? name : member.Username;
    }

    private async Task Send(DiscordClient client, string message) {
      var channel = await client.GetChannelAsync(_settings.Config.BotSpamChannel);
      await channel.SendMessageAsync(message);
    }
  }
}