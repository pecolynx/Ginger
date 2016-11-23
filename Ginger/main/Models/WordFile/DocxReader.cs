using log4net;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.Models.WordFile
{
    internal class DocxReader : IFileReaderBase
    {
        /// <summary>ロガー</summary>
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the <see cref="DocxReader"/> class.コンストラクタ</summary>
        /// <param name="filePath">ファイルパス</param>
        public DocxReader(string filePath)
        {
            this.FilePath = filePath;
        }

        /// <summary>Gets ファイルパス</summary>
        public string FilePath { get; private set; }

        public string Execute()
        {
            var sb = new StringBuilder();

            using (var fs = new FileStream(this.FilePath, FileMode.Open))
            {
                var document = new XWPFDocument(fs);

                foreach (var x in document.Paragraphs)
                {
                    var value = x.ParagraphText;
                    var trimmedValue = value.Trim();
                    if (!string.IsNullOrEmpty(trimmedValue))
                    {
                        sb.Append(trimmedValue);
                        sb.Append(" ");
                    }
                }

                foreach (var x in document.Tables)
                {
                    var value = x.Text;
                    var trimmedValue = value.Trim();
                    if (!string.IsNullOrEmpty(trimmedValue))
                    {
                        sb.Append(trimmedValue);
                        sb.Append(" ");
                    }
                }

                return sb.ToString();
            }
        }
    }
}
