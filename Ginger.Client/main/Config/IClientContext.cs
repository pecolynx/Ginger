using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.Client.Config
{
    public interface IClientContext
    {
        string ServerAddress { get; set; }
    }
}
