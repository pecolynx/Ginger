using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.Client.Exceptions
{
    /// <summary>
    /// 接続例外
    /// </summary>
    public class ConnectionException : Exception
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">メッセージ</param>
        public ConnectionException(string message)
            : base(message)
        {
        }
    }
}
