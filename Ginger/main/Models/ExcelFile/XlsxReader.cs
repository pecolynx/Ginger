using log4net;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.Models.ExcelFile
{
    internal class XlsxReader : IFileReaderBase
    {
        /// <summary>ロガー</summary>
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public XlsxReader(string filePath)
        {
            this.FilePath = filePath;
        }

        public string FilePath { get; private set; }

        public string Execute()
        {
            var sb = new StringBuilder();

            using (var fs = new FileStream(this.FilePath, FileMode.Open))
            {
                var workbook = new XSSFWorkbook(fs);
                for (var i = 0; i < workbook.NumberOfSheets; i++)
                {
                    var sheet = workbook.GetSheetAt(i);
                    for (var row = 0; row <= sheet.LastRowNum; row++)
                    {
                        var r = sheet.GetRow(row);
                        if (r == null)
                        {
                            continue;
                        }

                        for (var column = 0; column <= r.LastCellNum; column++)
                        {
                            var c = r.GetCell(column);
                            if (c == null)
                            {
                                continue;
                            }

                            var value = this.GetCellValue(c);
                            if (value == null)
                            {
                                continue;
                            }

                            var trimmedValue = value.Trim();
                            sb.Append(trimmedValue);
                            sb.Append(" ");
                        }
                    }
                }

                return sb.ToString();
            }
        }

        private string GetCellValue(ICell c)
        {
            switch (c.CellType)
            {
                case NPOI.SS.UserModel.CellType.String:
                    return c.StringCellValue;
                case NPOI.SS.UserModel.CellType.Numeric:
                    return c.NumericCellValue.ToString();
                case NPOI.SS.UserModel.CellType.Blank:
                    return string.Empty;
                default:
                    return string.Empty;
            }
        }
    }
}
