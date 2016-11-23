using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.Models.Io
{
    internal class DirectoryReader
    {
        public IEnumerable<string> ReadFileList(string directoryPath)
        {
            return Directory.EnumerateFiles(directoryPath, "*", SearchOption.AllDirectories);
        }
    }
}
