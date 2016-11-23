using Hnx8.ReadJEnc;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.Models.Io.TextFile
{
    internal class TextFileReader : IFileReaderBase
    {
        /// <summary>ロガー</summary>
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public TextFileReader(string filePath)
        {
            this.FilePath = filePath;
        }

        public string FilePath { get; private set; }

        public string Execute()
        {
            var fileInfo = new FileInfo(this.FilePath);
            using (var reader = new FileReader(fileInfo))
            {
                var charCode = reader.Read(fileInfo);
                if (charCode is CharCode.Text)
                {
                    return reader.Text;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}
