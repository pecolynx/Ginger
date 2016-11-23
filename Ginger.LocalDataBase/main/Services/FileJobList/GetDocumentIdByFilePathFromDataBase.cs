using Ginger.Common.Exceptions;
using Ginger.Common.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.LocalDataBase.Services
{
    public class GetDocumentIdByFilePathFromDataBase : DbServiceBase
    {
        public ModelId Execute(string filePath)
        {
            using (var conn = this.CreateConnection())
            {
                conn.Open();

                using (var command = new SQLiteCommand(conn))
                {
                    return this.ExecuteCommand(command, filePath);
                }
            }
        }

        private ModelId ExecuteCommand(SQLiteCommand command, string filePath)
        {
            command.CommandText =
                "SELECT " +
                "document_id " +
                "FROM " +
                "file_list " +
                "WHERE " +
                "file_path = @file_path ";

            command.Parameters.Add("file_path", System.Data.DbType.String);

            command.Parameters["file_path"].Value = filePath;

            using (var reader = command.ExecuteReader())
            {
                return this.ExecuteReader(reader);
            }
        }

        private ModelId ExecuteReader(SQLiteDataReader reader)
        {
            while (reader.Read())
            {
                var isNull = reader.IsDBNull(0);
                if (isNull)
                {
                    return null;
                }

                var value = reader.GetString(0);
                if (value == null || isNull)
                {
                    return null;
                }

                return new ModelId(value);
            }

            return null;
        }
    }
}
