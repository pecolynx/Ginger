using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.Client.Exceptions
{
    public class BadRequestException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BadRequestException"/> class.
        /// コンストラクタ
        /// </summary>
        /// <param name="message">メッセージ</param>
        public BadRequestException(string message)
            : base(message)
        {
        }
    }
}
