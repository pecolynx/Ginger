using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.LocalDataBase.Models
{
    public class FileDiff
    {
        public FileDiff(string filePath, DateTime dateTime1, DateTime dateTime2)
        {
            this.FilePath = filePath;
            this.DateTime1 = dateTime1;
            this.DateTime2 = dateTime2;
        }

        public string FilePath { get; set; }

        public DateTime DateTime1 { get; set; }

        public DateTime DateTime2 { get; set; }
    }
}
