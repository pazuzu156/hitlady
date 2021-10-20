using System.Text.RegularExpressions;
using DSharpPlus.Entities;

namespace Hitlady.Utils {
  public class MessageFilter {
    private DiscordGuild _guild;
    private DiscordMember _member;

    private MessageFilter(DiscordGuild guild, DiscordMember member) {
      _guild = guild;
      _member = member;
    }

    /// <summary>
    /// Initialize the message filter.
    /// </summary>
    /// <param name="guild"></param>
    /// <param name="member"></param>
    /// <returns></returns>
    public static MessageFilter init(DiscordGuild guild, DiscordMember member) {
      return new MessageFilter(guild, member);
    }

    /// <summary>
    /// Run a regex replace on a given string with a supplied regex string.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="regex"></param>
    /// <returns></returns>
    public string Replace(string message, string regex) {
      return Regex.Replace(message, regex, ReplacementCallback);
    }

    private string ReplacementCallback(Match match) {
      string r = string.Empty;

      switch(match.Groups[1].Value) {
        case "MENTION":
          r = _member.Mention;
          break;
        case "SERVERNAME":
          r = _guild.Name;
          break;
      }

      return r;
    }
  }
}
