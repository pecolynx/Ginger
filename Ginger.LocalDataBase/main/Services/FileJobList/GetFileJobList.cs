using Ginger.LocalDataBase.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.LocalDataBase.Services
{
    public class GetFileJobList : DbServiceBase
    {
        public List<DbFileJob> Execute()
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

        private List<DbFileJob> ExecuteCommand(SQLiteCommand command)
        {
            command.CommandText =
                "SELECT " +
                "unique_key " +
                ",file_path " +
                ",command " +
                ",created_at " +
                ",updated_at " +
                "FROM " +
                "file_job_list " +
                "order by created_at";

            using (var reader = command.ExecuteReader())
            {
                return this.ExecuteReader(reader);
            }
        }

        private List<DbFileJob> ExecuteReader(SQLiteDataReader reader)
        {
            var fileJobList = new List<DbFileJob>();

            while (reader.Read())
            {
                var key = reader.GetString(0);
                var filePath = reader.GetString(1);
                var command = reader.GetInt32(2);
                var createdAt = reader.GetDateTime(3);
                var updatedAt = reader.GetDateTime(4);

                fileJobList.Add(new DbFileJob(key, filePath, command, createdAt, updatedAt));
            }

            return fileJobList;
        }
    }
}
