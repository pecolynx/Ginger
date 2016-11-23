using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.ViewModels
{
    internal class VmDocumentFile
    {
        /// <summary>ロガー</summary>
        private static readonly ILog L = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public VmDocumentFile(string id, string fileName, string filePath, string fileContent, string fileHash, DateTime fileCreatedAt, DateTime fileUpdatedAt)
        {
            this.Id = id;
            this.FileName = fileName;
            this.FilePath = filePath;
            this.FileContent = fileContent;
            this.FileHash = fileHash;
            this.FileCreatedAt = fileCreatedAt;
            this.FileUpdatedAt = fileUpdatedAt;
        }

        public string Id { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string FileContent { get; set; }

        public string FileHash { get; set; }

        public DateTime FileCreatedAt { get; private set; }

        public DateTime FileUpdatedAt { get; private set; }

        public string FileContentForView
        {
            get
            {
                var content = this.FileContent
                    .Replace("\r\n", " ")
                    .Replace("\n", " ")
                    .Replace("\t", " ")
                    .Replace("　", " ")
                    .Replace("  ", " ");
                if (content.Length > 200)
                {
                    return content.Substring(0, 200);
                }
                else
                {
                    return content;
                }
            }

            set
            {
            }
        }

        public string FileCreatedAtForView
        {
            get
            {
                return "作成日時 : " + this.FileCreatedAt.ToString();
            }

            set
            {
            }
        }

        public string FileUpdatedAtForView
        {
            get
            {
                return "更新日時 : " + this.FileUpdatedAt.ToString();
            }

            set
            {
            }
        }
    }
}
