using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.LocalDataBase.Services
{
    public class ClearDb : DbServiceBase
    {
        public void Execute()
        {
            if (File.Exists(this.LocalDataBaseContext.DataBaseFilePath))
            {
                File.Delete(this.LocalDataBaseContext.DataBaseFilePath);
            }
        }
    }
}
