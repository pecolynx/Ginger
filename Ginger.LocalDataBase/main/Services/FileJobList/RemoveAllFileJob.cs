using log4net;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.LocalDataBase.Services
{
    public class RemoveAllFileJob : DbServiceBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

        private void ExecuteCommand(SQLiteCommand command)
        {
            command.CommandText =
                "DELETE FROM file_job_list ";
            command.ExecuteNonQuery();
        }
    }
}
