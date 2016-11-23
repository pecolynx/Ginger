using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.LocalDataBase.Services
{
    public class AddFileToDataBase : DbServiceBase
    {
        private const string PARAM_FILE_PATH = "file_path";

        public void Execute(string filePath, string documentId, DateTime fileCreatedAt, DateTime fileUpdatedAt)
        {
            using (var conn = this.CreateConnection())
            {
                conn.Open();

                using (var command = new SQLiteCommand(conn))
                {
                    this.ExecuteCommand(command, filePath, documentId, fileCreatedAt, fileUpdatedAt);
                }
            }
        }

        private void ExecuteCommand(SQLiteCommand command, string filePath, string documentId, DateTime fileCreatedAt, DateTime fileUpdatedAt)
        {
            command.CommandText =
                "INSERT INTO file_list ( " +
                "file_path" +
                ",document_id" +
                ",file_created_at" +
                ",file_updated_at" +
                ",created_at" +
                ",updated_at" +
                ") VALUES (" +
                "@file_path" +
                ",@document_id" +
                ",@file_created_at" +
                ",@file_updated_at" +
                ",@created_at" +
                ",@updated_at" +
                ");";

            command.Parameters.Add("file_path", System.Data.DbType.String);
            command.Parameters.Add("document_id", System.Data.DbType.String);
            command.Parameters.Add("created_at", System.Data.DbType.DateTime);
            command.Parameters.Add("updated_at", System.Data.DbType.DateTime);
            command.Parameters.Add("file_created_at", System.Data.DbType.DateTime);
            command.Parameters.Add("file_updated_at", System.Data.DbType.DateTime);

            command.Parameters["file_path"].Value = filePath;
            command.Parameters["document_id"].Value = documentId;
            command.Parameters["created_at"].Value = fileCreatedAt;
            command.Parameters["file_created_at"].Value = fileCreatedAt;
            command.Parameters["file_updated_at"].Value = fileUpdatedAt;
            command.Parameters["created_at"].Value = DateTime.Now;
            command.Parameters["updated_at"].Value = DateTime.Now;

            command.ExecuteNonQuery();
        }
    }
}
