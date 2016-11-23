using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.LocalDataBase.Services.LocalFile
{
    public class AddLocalFile : DbServiceBase
    {
        private const string PARAM_FILE_PATH = "file_path";
        private const string PARAM_FILE_CREATED_AT = "file_created_at";
        private const string PARAM_FILE_UPDATED_AT = "file_updated_at";

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
                "INSERT INTO local_file_list ( " +
                "file_path" +
                ",file_created_at" +
                ",file_updated_at" +
                ") VALUES (" +
                "@" + PARAM_FILE_PATH + " " +
                ",@" + PARAM_FILE_CREATED_AT + " " +
                ",@" + PARAM_FILE_UPDATED_AT + " " +
                ");";

            command.Parameters.Add(PARAM_FILE_PATH, System.Data.DbType.String);
            command.Parameters.Add(PARAM_FILE_CREATED_AT, System.Data.DbType.DateTime);
            command.Parameters.Add(PARAM_FILE_UPDATED_AT, System.Data.DbType.DateTime);

            command.Parameters[PARAM_FILE_PATH].Value = filePath;
            command.Parameters[PARAM_FILE_CREATED_AT].Value = fileCreatedAt;
            command.Parameters[PARAM_FILE_UPDATED_AT].Value = fileUpdatedAt;

            command.ExecuteNonQuery();
        }
    }
}
