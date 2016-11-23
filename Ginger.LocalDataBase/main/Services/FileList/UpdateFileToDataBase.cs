using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.LocalDataBase.Services
{
    public class UpdateFileToDataBase : DbServiceBase
    {
        private const string PARAM_FILE_PATH = "file_path";

        public void Execute(string filePath, DateTime fileCreatedAt, DateTime fileUpdatedAt)
        {
            using (var conn = this.CreateConnection())
            {
                conn.Open();

                using (var command = new SQLiteCommand(conn))
                {
                    this.ExecuteCommand(command, filePath, fileCreatedAt, fileUpdatedAt);
                }
            }
        }

        private void ExecuteCommand(SQLiteCommand command, string filePath, DateTime fileCreatedAt, DateTime fileUpdatedAt)
        {
            command.CommandText =
                "UPDATE file_list " +
                "SET " +
                "file_created_at = @file_created_at " +
                ",file_updated_at = @file_updated_at " +
                "WHERE " +
                "file_path = @" + PARAM_FILE_PATH + " ";

            command.Parameters.Add(PARAM_FILE_PATH, System.Data.DbType.String);
            command.Parameters.Add("file_created_at", System.Data.DbType.DateTime);
            command.Parameters.Add("file_updated_at", System.Data.DbType.DateTime);

            command.Parameters[PARAM_FILE_PATH].Value = filePath;
            command.Parameters["file_created_at"].Value = fileCreatedAt;
            command.Parameters["file_updated_at"].Value = fileUpdatedAt;

            command.ExecuteNonQuery();
        }
    }
}
