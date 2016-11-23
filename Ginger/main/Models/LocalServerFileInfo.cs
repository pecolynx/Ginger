using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.Models
{
    public class LocalServerFileInfo
    {
        public LocalServerFileInfo(string filePath, string documentId, string dateTime1, string dateTime2, string status)
        {
            this.FilePath = filePath;
            this.DocumentId = documentId;
            this.DateTime1 = dateTime1;
            this.DateTime2 = dateTime2;
            this.Status = status;
        }

        public string FilePath { get; set; }

        public string DocumentId { get; set; }

        public string DateTime1 { get; set; }

        public string DateTime2 { get; set; }

        public string Status { get; set; }
    }
}
