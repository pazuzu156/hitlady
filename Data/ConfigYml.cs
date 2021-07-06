using System.Collections.Generic;

namespace Hitlady.Data {
  internal struct ConfigYml {
    public string Token { get; set; }
    public string Prefix { get; set; }
    public Channels Channels { get; set; }
    public Database Database { get; set; }
    public int LogLevel { get; set; }
  }

  internal struct Channels {
    public ulong BotSpam { get; set; }
  }

  internal struct Database {
    public string Hostname { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
  }
}
