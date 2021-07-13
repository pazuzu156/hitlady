using System.Collections.Generic;
using YamlDotNet.Core.Tokens;

namespace Hitlady.Data {
  public struct ConfigYml {
    public string Token { get; set; }
    public string Prefix { get; set; }
    public Channels Channels { get; set; }
    public Database Database { get; set; }
    public int LogLevel { get; set; }
    public List<ulong> JoinableRoles { get; set; }
    public Messages Messages { get; set; }
  }

  public struct Channels {
    public ulong BotSpam { get; set; }
  }

  public struct Database {
    public string Hostname { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
  }

  public struct Messages {
    public List<string> Join { get; set; }
    public List<string> Leave { get; set; }
  }
}
