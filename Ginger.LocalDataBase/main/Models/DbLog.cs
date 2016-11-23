using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.LocalDataBase.Models
{
    public class DbLog
    {
        public DbLog(string level, string message, DateTime createdAt)
        {
            this.Level = level;
            this.Message = message;
            this.CreatedAt = createdAt;
        }

        public string Level { get; private set; }

        public string Message { get; private set; }

        public DateTime CreatedAt { get; private set; }
    }
}
