using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.LocalDataBase.Services.LogList
{
    public class AddLog : DbServiceBase
    {
        private const string PARAM_LEVEL = "level";
        private const string PARAM_MESSAGE = "message";
        private const string PARAM_CREATED_AT = "created_at";

        public void Execute(string level, string message)
        {
            using (var conn = this.CreateConnection())
            {
                conn.Open();

                using (var command = new SQLiteCommand(conn))
                {
                    this.ExecuteCommand(command, level, message);
                }
            }
        }

        private void ExecuteCommand(SQLiteCommand command, string level, string message)
        {
            command.CommandText =
                "INSERT INTO log_list ( " +
                "level" +
                ",message" +
                ",created_at" +
                ") VALUES (" +
                "@" + PARAM_LEVEL + " " +
                ",@" + PARAM_MESSAGE + " " +
                ",@" + PARAM_CREATED_AT + " " +
                ");";

            command.Parameters.Add(PARAM_LEVEL, System.Data.DbType.String);
            command.Parameters.Add(PARAM_MESSAGE, System.Data.DbType.String);
            command.Parameters.Add(PARAM_CREATED_AT, System.Data.DbType.DateTime);

            command.Parameters[PARAM_LEVEL].Value = level;
            command.Parameters[PARAM_MESSAGE].Value = message;
            command.Parameters[PARAM_CREATED_AT].Value = default(DateTime);

            command.ExecuteNonQuery();
        }
    }
}
