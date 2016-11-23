using System;
using System.Threading;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Ginger.LocalDataBase.Services
{
    public class AddFileJobToDataBase : DbServiceBase
    {
        private const string PARAM_FILE_PATH = "file_path";

        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(string filePath, int command)
        {
            using (var conn = this.CreateConnection())
            {
                conn.Open();

                using (var sqlCommand = new SQLiteCommand(conn))
                {
                    this.ExecuteCommand(sqlCommand, filePath, command);
                }
            }
        }

        private void ExecuteCommand(SQLiteCommand sqlCommand, string filePath, int command)
        {
            sqlCommand.CommandText =
                "INSERT INTO file_job_list ( " +
                "unique_key " +
                ",file_path" +
                ",command" +
                ",created_at" +
                ",updated_at" +
                ") VALUES (" +
                "@unique_key" +
                ",@file_path" +
                ",@command" +
                ",@created_at" +
                ",@updated_at" +
                ");";

            sqlCommand.Parameters.Add("unique_key", System.Data.DbType.String);
            sqlCommand.Parameters.Add("file_path", System.Data.DbType.String);
            sqlCommand.Parameters.Add("command", System.Data.DbType.Int32);
            sqlCommand.Parameters.Add("created_at", System.Data.DbType.DateTime);
            sqlCommand.Parameters.Add("updated_at", System.Data.DbType.DateTime);

            var x = Guid.NewGuid().ToString("N");
            Logger.Warn("guid = " + x);
            sqlCommand.Parameters["unique_key"].Value = Guid.NewGuid().ToString("N");
            sqlCommand.Parameters["file_path"].Value = filePath;
            sqlCommand.Parameters["command"].Value = command;
            sqlCommand.Parameters["created_at"].Value = DateTime.Now;
            sqlCommand.Parameters["updated_at"].Value = DateTime.Now;

            sqlCommand.ExecuteNonQuery();

            Thread.Sleep(50);
        }
    }
}
