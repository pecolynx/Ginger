using Ginger.LocalDataBase.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.LocalDataBase.Services
{
    public class GetFileInfoListToBeUpdated : DbServiceBase
    {
        public List<FileDiff> Execute()
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

        private List<FileDiff> ExecuteCommand(SQLiteCommand command)
        {
            command.CommandText =
                "SELECT t1.file_path, t1.file_created_at, t2.file_created_at FROM file_list t1 " +
                "INNER JOIN local_file_list t2 " +
                "ON t1.file_path = t2.file_path " +
                "AND datetime(t1.file_updated_at) <> datetime(t2.file_updated_at) ";

            using (var reader = command.ExecuteReader())
            {
                var list = new List<FileDiff>();
                while (reader.Read())
                {
                    var filePath = reader.GetString(0);
                    var dateTime1 = reader.GetDateTime(1);
                    var dateTime2 = reader.GetDateTime(2);

                    list.Add(new Models.FileDiff(filePath, dateTime1, dateTime2));
                }

                return list;
            }
        }
    }
}
