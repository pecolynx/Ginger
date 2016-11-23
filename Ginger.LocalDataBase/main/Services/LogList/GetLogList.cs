using Ginger.LocalDataBase.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.LocalDataBase.Services.LogList
{
    public class GetLogList : DbServiceBase
    {
        public List<DbLog> Execute()
        {
            using (var conn = this.CreateConnection())
            {
                conn.Open();

                using (var command = new SQLiteCommand(conn))
                {
                    return this.ExecuteCommand(command);
                }
            }
        }

        private List<DbLog> ExecuteCommand(SQLiteCommand command)
        {
            command.CommandText =
                "SELECT level, message, created_at " +
                "FROM log_list ";

            using (var reader = command.ExecuteReader())
            {
                return this.ReadDbLogList(reader);
            }
        }

        private List<DbLog> ReadDbLogList(SQLiteDataReader reader)
        {
            var list = new List<DbLog>();

            while (reader.Read())
            {
                var level = reader.GetString(0);
                var message = reader.GetString(1);
                var createdAt = reader.GetDateTime(2);

                list.Add(new DbLog(level, message, createdAt));
            }

            return list;
        }
    }
}
