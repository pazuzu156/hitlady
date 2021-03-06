using Hitlady.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Hitlady.Utils {
  internal class Settings {
    private string _settingsFile = "config.yml";
    public ConfigYml Config;

    public bool Generated = false;

    public static Settings GetInstance() => new Settings();

    private Settings() {
      _settingsFile = Path.GetFileName("./") + _settingsFile;

      if (exists()) {
        load();
      } else {
        create();
      }
    }

    private bool exists() => File.Exists(_settingsFile);

    private void create() {
      Console.Write("Your bot's token: ");
      var token = Console.ReadLine();
      Console.Write("Bot spam channel ID: ");
      var bs = Console.ReadLine();
      Console.Write("Prefix: [-] ");
      var prefix = Console.ReadLine();
      Console.Write("Database hostname: [localhost] ");
      var dbh = Console.ReadLine();
      Console.Write("Database port: [3306] ");
      var dbp = Console.ReadLine();
      Console.Write("Database username: [root] ");
      var dbu = Console.ReadLine();
      Console.Write("Database password: [leave blank for no password] ");
      var dbpw = Console.ReadLine();
      Console.Write("Database name: [hitlady] ");
      var dbn = Console.ReadLine();

      if (string.IsNullOrEmpty(bs)) {
        bs = "0";
      }

      ulong bsu = 0;
      int dbpi = 0;
      UInt64.TryParse(bs, NumberStyles.Integer, CultureInfo.CurrentCulture, out bsu);
      Int32.TryParse(dbp, NumberStyles.Integer, CultureInfo.CurrentCulture, out dbpi);

      Generated = true;
      var pl = (string.IsNullOrEmpty(prefix)) ? "!" : prefix;

      using (var writer = new StreamWriter(File.Create(_settingsFile))) {
        var data = new ConfigYml {
          Token = token,
          Prefix = pl,
          Channels = new Channels {
            BotSpam = bsu
          },
          Database = new Database {
            Hostname = (string.IsNullOrEmpty(dbh)) ? "localhost" : dbh,
            Port = (dbpi == 0) ? 3306 : dbpi,
            Username = (string.IsNullOrEmpty(dbu)) ? "root" : dbu,
            Password = (string.IsNullOrEmpty(dbpw)) ? string.Empty : dbpw,
            Name = (string.IsNullOrEmpty(dbn)) ? "hitlady" : dbn
          },
          Lastfm = new Data.Lastfm {
            ApiKey = string.Empty,
            ApiSecret = string.Empty
          },
          YouTube = new YouTube {
            ApiKey = string.Empty
          },
          LogLevel = 2,
          JoinableRoles = new List<ulong>{},
          Messages = new Messages {
            Join = new List<string>{ "PLEASE PLACE WELCOME MESSAGES IN HERE" },
            Leave = new List<string>{ "PLEASE PLACE LEAVE MESSAGES IN HERE" },
          }
        };

        var se = new SerializerBuilder()
          .WithNamingConvention(NullNamingConvention.Instance)
          .Build();

        se.Serialize(writer, data);
      }
    }

    private void load() {
      using (var reader = new StreamReader(File.OpenRead(_settingsFile))) {
        var ds = new DeserializerBuilder()
          .WithNamingConvention(NullNamingConvention.Instance)
          .Build();
        Config = ds.Deserialize<ConfigYml>(reader.ReadToEnd());
      }
    }
  }
}
