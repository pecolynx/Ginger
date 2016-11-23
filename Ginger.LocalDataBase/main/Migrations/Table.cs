using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.LocalDataBase.Migrations
{
    public class Table
    {
        public const string SQL_001_CREATE_FILE_LIST =
            "CREATE TABLE IF NOT EXISTS file_list ( " +
            "file_path NVARCHAR NOT NULL" +
            ",document_id NVARCHAR" +
            ",file_created_at TIMESTAMP NOT NULL" +
            ",file_updated_at TIMESTAMP NOT NULL" +
            ",created_at TIMESTAMP NOT NULL" +
            ",updated_at TIMESTAMP NOT NULL" +
            ");";

        public const string SQL_002_CREATE_FILE_JOB_LIST =
            "CREATE TABLE IF NOT EXISTS file_job_list (" +
            "unique_key NVARCHAR NOT NULL" +
            ",file_path NVARCHAR NOT NULL" +
            ",command INTEGER NOT NULL" +
            ",created_at TIMESTAMP NOT NULL" +
            ",updated_at TIMESTAMP NOT NULL" +
            ")";

        public const string SQL_003_CREATE_LOCAL_FILE_LIST =
            "CREATE TABLE IF NOT EXISTS local_file_list ( " +
            "file_path NVARCHAR NOT NULL" +
            ",file_created_at TIMESTAMP NOT NULL" +
            ",file_updated_at TIMESTAMP NOT NULL" +
            ");";

        public const string SQL_004_CREATE_SERVER_FILE_LIST =
            "CREATE TABLE IF NOT EXISTS server_file_list ( " +
            "file_path NVARCHAR NOT NULL " +
            ");";

        public const string SQL_101_CREATE_LOG_LIST =
            "CREATE TABLE IF NOT EXISTS log_list ( " +
            "level NVARCHAR NOT NULL " +
            ",message NVARCHAR NOT NULL " +
            ",created_at TIMESTAMP NOT NULL" +
            ");";

        public static readonly List<string> MIGRATION_LIST = new List<string>()
        {
            SQL_001_CREATE_FILE_LIST,
            SQL_002_CREATE_FILE_JOB_LIST,
            SQL_003_CREATE_LOCAL_FILE_LIST,
            SQL_004_CREATE_SERVER_FILE_LIST,
            SQL_101_CREATE_LOG_LIST,
        };
    }
}
