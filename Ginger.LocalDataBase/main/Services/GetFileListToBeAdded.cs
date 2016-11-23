using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.LocalDataBase.Services
{
    public class GetFileListToBeAdded : DbServiceBase
    {
        public List<string> Execute()
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

        private List<string> ExecuteCommand(SQLiteCommand command)
        {
            command.CommandText =
                "SELECT t1.file_path " +
                "FROM local_file_list t1 " +
                "WHERE NOT EXISTS ( " +
                "  SELECT * FROM server_file_list t2 " +
                "  WHERE t1.file_path = t2.file_path " +
                ") ";

            using (var reader = command.ExecuteReader())
            {
                return SqlReaderUtils.ReadStringList(reader);
            }
        }
    }
}
