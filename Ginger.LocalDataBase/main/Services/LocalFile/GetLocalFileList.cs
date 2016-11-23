using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.LocalDataBase.Services.LocalFile
{
    public class GetLocalFileList : DbServiceBase
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
                "FROM local_file_list t1 ";

            using (var reader = command.ExecuteReader())
            {
                return SqlReaderUtils.ReadStringList(reader);
            }
        }
    }
}
