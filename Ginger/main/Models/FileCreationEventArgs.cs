using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.Models.Io
{
    /// <summary>
    /// ファイル生成
    /// </summary>
    public class FileCreationEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileCreationEventArgs"/> class.
        /// パス情報を指定して、FileCreationEventArgs クラスのインスタンスを生成、初期化します。
        /// </summary>
        /// <param name="fullpath">パス情報</param>
        public FileCreationEventArgs(string fullpath)
        {
            this.FullPath = fullpath;
        }

        /// <summary>
        /// 生成されたファイルへのフルパスを取得します。
        /// </summary>
        public string FullPath { get; private set; }
    }
}
