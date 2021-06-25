using System;
using System.Reflection;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using Hitlady.Commands;
using Hitlady.Data;
using Hitlady.Utils;
using Microsoft.Extensions.Logging;

namespace Hitlady {
  class Program {
    /// <summary>
    /// Bot's Verson.
    /// </summary>
    public static Version Version { get; private set; }

    /// <summary>
    /// App starting timestamp.
    /// </summary>
    public static DateTime StartTime { get; private set; }

    public static ConfigYml Config { get; private set; }

    /// <summary>
    /// Discord client instance.
    /// </summary>
    private DiscordClient _client;

    /// <summary>
    /// Commands extension instance.
    /// </summary>
    private CommandsNextExtension _commands;

    static void Main(string[] args) {
      Version = Assembly.GetExecutingAssembly().GetName().Version;
      StartTime = DateTime.UtcNow;

      Config = Settings.GetInstance().Config;

      new Program().RunBotAsync().GetAwaiter().GetResult();
    }

    public async Task RunBotAsync() {
      _client = new DiscordClient(new DiscordConfiguration {
        Token = Config.Token,
        TokenType = TokenType.Bot,
        Intents = DiscordIntents.AllUnprivileged | DiscordIntents.GuildMembers,
        AutoReconnect = true,
        MinimumLogLevel = (LogLevel) Config.LogLevel
      });

      var bot = new BotClient(_client);

      _commands = _client.UseCommandsNext(new CommandsNextConfiguration {
        StringPrefixes = Config.Prefixes,
      });
      _commands.CommandExecuted += bot.Commands_CommandExecuted;

      // all command modules should be registered here
      _commands.RegisterCommands<AboutModule>();

      await _client.ConnectAsync();
      await Task.Delay(-1);
    }
  }
}
