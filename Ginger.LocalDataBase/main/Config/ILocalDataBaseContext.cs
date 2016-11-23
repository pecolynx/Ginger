using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.LocalDataBase.Config
{
    public interface ILocalDataBaseContext
    {
        string DataBaseFilePath { get; }
    }
}
