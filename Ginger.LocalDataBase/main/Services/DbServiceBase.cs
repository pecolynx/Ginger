using Common.Logging;
using Ginger.LocalDataBase.Config;
using System.Data.SQLite;

namespace Ginger.LocalDataBase.Services
{
    public class DbServiceBase
    {
        /// <summary>ロガー</summary>
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ILocalDataBaseContext LocalDataBaseContext { get; set; }

        /// <summary>
        /// Dbコネクションを作成します。
        /// </summary>
        /// <returns>Dbコネクション</returns>
        protected SQLiteConnection CreateConnection()
        {
            var connStr = new SQLiteConnectionStringBuilder()
            {
                DataSource = this.LocalDataBaseContext.DataBaseFilePath
            };

            return new SQLiteConnection(connStr.ToString());
        }
    }
}
