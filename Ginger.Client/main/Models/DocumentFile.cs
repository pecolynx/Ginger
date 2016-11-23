using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.Client.Models
{
    public class DocumentFile : Document
    {
        /// <summary>ロガー</summary>
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DocumentFile()
        {
            this.FileName = string.Empty;
            this.FilePath = string.Empty;
            this.FileContent = string.Empty;
            this.FileHash = string.Empty;
            this.FileCreatedAt = default(DateTime);
            this.FileUpdatedAt = default(DateTime);
        }

        public DocumentFile(string fileName, string filePath, string fileContent, string fileHash, DateTime fileCreatedAt, DateTime fileUpdatedAt)
        {
            this.FileName = fileName;
            this.FilePath = filePath;
            this.FileContent = fileContent;
            this.FileHash = fileHash;
            this.FileCreatedAt = fileCreatedAt;
            this.FileUpdatedAt = fileUpdatedAt;
        }

        public DocumentFile(string id, string fileName, string filePath, string fileContent, string fileHash, DateTime fileCreatedAt, DateTime fileUpdatedAt)
            : base(id)
        {
            this.FileName = fileName;
            this.FilePath = filePath;
            this.FileContent = fileContent;
            this.FileHash = fileHash;
            this.FileCreatedAt = fileCreatedAt;
            this.FileUpdatedAt = fileUpdatedAt;
        }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string FileContent { get; set; }

        public string FileHash { get; set; }

        public DateTime FileCreatedAt { get; private set; }

        public DateTime FileUpdatedAt { get; private set; }
    }
}
