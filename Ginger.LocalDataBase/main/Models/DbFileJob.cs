using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.LocalDataBase.Models
{
    public class DbFileJob
    {
        public DbFileJob(string uniqueKey, string filePath, int command, DateTime createdAt, DateTime updatedAt)
        {
            this.UniqueKey = uniqueKey;
            this.FilePath = filePath;
            this.Command = command;
        }

        public string UniqueKey { get; set; }

        public string FilePath { get; set; }

        public int Command { get; set; }
    }
}
