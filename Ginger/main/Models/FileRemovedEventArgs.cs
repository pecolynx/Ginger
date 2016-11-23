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
    public class FileRemovedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileRemovedEventArgs"/> class.
        /// パス情報を指定して、FileCreationEventArgs クラスのインスタンスを生成、初期化します。
        /// </summary>
        /// <param name="fullpath">パス情報</param>
        public FileRemovedEventArgs(string fullpath)
        {
            this.FullPath = fullpath;
        }

        /// <summary>
        /// Gets 生成されたファイルへのフルパス
        /// </summary>
        public string FullPath { get; private set; }
    }
}
