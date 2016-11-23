using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.Client.Exceptions
{
    /// <summary>
    /// 認証例外クラス
    /// </summary>
    public class AuthException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthException"/> class.
        /// コンストラクタ
        /// </summary>
        /// <param name="message">メッセージ</param>
        public AuthException(string message)
            : base(message)
        {
        }
    }
}
