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
    public class FileUpdatedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileUpdatedEventArgs"/> class.
        /// パス情報を指定して、FileCreationEventArgs クラスのインスタンスを生成、初期化します。
        /// </summary>
        /// <param name="fullpath">パス情報</param>
        public FileUpdatedEventArgs(string fullpath)
        {
            this.FullPath = fullpath;
        }

        /// <summary>
        /// Gets 生成されたファイルへのフルパス
        /// </summary>
        public string FullPath { get; private set; }
    }
}
