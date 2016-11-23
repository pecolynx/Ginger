using Ginger.LocalDataBase.Migrations;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.LocalDataBase.Services
{
    public class MigrateDataBase : DbServiceBase
    {
        public void Execute()
        {
            using (var conn = this.CreateConnection())
            {
                conn.Open();

                using (var command = new SQLiteCommand(conn))
                {
                    this.ExecuteCommand(command);
                }
            }
        }

        public void ExecuteCommand(SQLiteCommand command)
        {
            foreach (var sql in Table.MIGRATION_LIST)
            {
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
        }
    }
}
