using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.LocalDataBase.Services
{
    internal class SqlReaderUtils
    {
        public static List<string> ReadStringList(SQLiteDataReader reader)
        {
            var list = new List<string>();

            while (reader.Read())
            {
                list.Add(reader.GetString(0));
            }

            return list;
        }
    }
}
