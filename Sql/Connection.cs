using ServiceStack.OrmLite;
using System.Data;
using System.Threading.Tasks;

namespace Hitlady.Sql {
  public class Connection {
    public static async Task<IDbConnection> Connect() {
      var dbc = Program.Config.Database;
      var dsn = $"Server={dbc.Hostname};Port={dbc.Port};UID={dbc.Username};Password={dbc.Password};Database={dbc.Name};SslMode=None";
      var factory = new OrmLiteConnectionFactory(dsn, MySqlDialect.Provider);

      return await OrmLiteConnectionFactoryExtensions.OpenDbConnectionAsync(factory, null);
    }
  }
}
