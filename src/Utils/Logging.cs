using System.Threading.Tasks;
using DSharpPlus;
using Microsoft.Extensions.Logging;

namespace Hitlady.Utils {
  public class Logging {
    private ILogger<BaseDiscordClient> _logger;

    public Logging(DiscordClient client) {
     this._logger = client.Logger;
    }

    public Task Debug(string message) {
      this._logger.LogDebug(message);

      return Task.CompletedTask;
    }

    public Task Info(string message) {
      this._logger.LogInformation(message);

      return Task.CompletedTask;
    }

    public Task Error(string message) {
      this._logger.LogError(message);

      return Task.CompletedTask;
    }
  }
}
