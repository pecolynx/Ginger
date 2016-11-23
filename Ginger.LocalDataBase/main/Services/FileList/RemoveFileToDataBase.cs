using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.LocalDataBase.Services
{
    public class RemoveFileToDataBase : DbServiceBase
    {
        private const string PARAM_FILE_PATH = "file_path";

        public void Execute(string filePath)
        {
            using (var conn = this.CreateConnection())
            {
                conn.Open();

                using (var command = new SQLiteCommand(conn))
                {
                    this.ExecuteCommand(command, filePath);
                }
            }
        }

        private void ExecuteCommand(SQLiteCommand command, string filePath)
        {
            command.CommandText =
                "DELETE FROM file_list " +
                "WHERE " +
                "file_path = @" + PARAM_FILE_PATH + " ";

            command.Parameters.Add(PARAM_FILE_PATH, System.Data.DbType.String);

            command.Parameters[PARAM_FILE_PATH].Value = filePath;

            command.ExecuteNonQuery();
        }
    }
}
