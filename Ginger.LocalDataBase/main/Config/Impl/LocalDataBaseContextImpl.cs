using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.LocalDataBase.Config
{
    public class LocalDataBaseContextImpl : ILocalDataBaseContext
    {
        public string DataBaseFilePath
        {
            get
            {
                return @"C:\Test.db";
            }
        }
    }
}
