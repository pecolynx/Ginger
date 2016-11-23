using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.LocalDataBase.Services.ServerFile
{
    public class RemoveAllServerFile : DbServiceBase
    {
        public void Execute()
        {
            using (var conn = this.CreateConnection())
            {
                conn.Open();

                using (var sqlCommand = new SQLiteCommand(conn))
                {
                    this.ExecuteCommand(sqlCommand);
                }
            }
        }

        private void ExecuteCommand(SQLiteCommand sqlCommand)
        {
            sqlCommand.CommandText =
                "DELETE FROM server_file_list ";

            sqlCommand.ExecuteNonQuery();
        }
    }
}
