using Ginger.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;

namespace Ginger.Models.Io
{/// <summary>
 /// ファイル生成を監視
 /// </summary>
    public class FileWatchTrigger : IDisposable
    {
        /// <summary>
        /// 監視する対象のフィルタ条件
        /// </summary>
        private string filter = string.Empty;

        /// <summary>
        /// サブディレクトリを検索するかどうか
        /// </summary>
        private bool includeSubdirectories = true;

        /// <summary>
        /// 監視する間隔
        /// </summary>
        private double interval = 0;

        /// <summary>
        /// 監視する対象のフォルダパス
        /// </summary>
        private string path = string.Empty;

        /// <summary>
        /// Timerクラスのインスタンス
        /// </summary>
        private Timer timer;

        /// <summary>
        /// FileSystemWatcherクラスのインスタンス
        /// </summary>
        private FileSystemWatcher watcher;

        /// <summary>
        /// 生成されたファイルでまだイベントを発生させていないファイルのリスト
        /// </summary>
        private List<string> createdWatchList = new List<string>();

        private List<string> deletedWatchList = new List<string>();

        private List<string> changedWatchList = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileWatchTrigger"/> class.
        /// 監視ディレクトリ、監視ファイルのフィルタ条件、サブディレクトリを監視するかどうか を指定して
        /// FileCreationTrigger クラスのインスタンスを生成、初期化します。
        /// </summary>
        /// <param name="path">監視ディレクトリ</param>
        /// <param name="fileter">監視ファイルのフィルタ条件</param>
        /// <param name="includeSubdirectories">サブディレクトリを監視するかどうか</param>
        /// <param name="synchronizingObject">同期</param>
        public FileWatchTrigger(string path, string fileter, bool includeSubdirectories, ISynchronizeInvoke synchronizingObject)
        {
            this.InitializeInstance(path, fileter, includeSubdirectories, null, synchronizingObject);
        }

        // ---------------------------------------
        //  イベント
        // ---------------------------------------

        /// <summary>
        /// 監視対象ディレクトリに指定されたフィルタに該当するファイルが生成されたとき発生します。
        /// メインスレッドとは別スレッドで動作する場合があります。
        /// </summary>
        public event EventHandler<FileCreationEventArgs> FileAdded;

        public event EventHandler<FileRemovedEventArgs> FileRemoved;

        public event EventHandler<FileUpdatedEventArgs> FileUpdated;

        // ---------------------------------------
        //  プロパティ
        // ---------------------------------------

        /// <summary>
        /// Gets or sets 監視対象ファイルのフィルタ条件を取得または設定します。
        /// </summary>
        public string Filter
        {
            get
            {
                return this.filter;
            }

            set
            {
                this.filter = value;
                this.watcher.Filter = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether サブディレクトリを監視するかどうか
        /// </summary>
        public bool IncludeSubdirectories
        {
            get
            {
                return this.includeSubdirectories;
            }

            set
            {
                this.includeSubdirectories = value;
                this.watcher.IncludeSubdirectories = value;
            }
        }

        /// <summary>
        /// Gets or sets 監視間隔(msec)
        /// </summary>
        public double Interval
        {
            get
            {
                return this.interval;
            }

            set
            {
                this.interval = value;
                this.timer.Interval = value;
            }
        }

        /// <summary>
        /// Gets or sets 監視対象ディレクトリのパス
        /// </summary>
        public string Path
        {
            get
            {
                return this.path;
            }

            set
            {
                this.path = value;
                this.watcher.Path = value;
            }
        }

        // ---------------------------------------
        //  メソッド
        // ---------------------------------------

        /// <summary>
        /// インスタンスを廃棄します。
        /// </summary>
        public void Dispose()
        {
            this.watcher.Dispose();
            this.timer.Dispose();
        }

        /// <summary>
        /// Fireイベントの発生を開始します。
        /// </summary>
        public void Start()
        {
            this.watcher.EnableRaisingEvents = true;
            this.timer.Start();
        }

        /// <summary>
        /// Fireイベントの発生を停止します。
        /// </summary>
        public void Stop()
        {
            this.watcher.EnableRaisingEvents = false;
            this.timer.Stop();
        }

        /// <summary>
        /// FileSystemWatcherでCreatedイベントが発生したとき実行されます。
        /// </summary>
        /// <param name="sender">呼び出し元インスタンス</param>
        /// <param name="e">イベント引数</param>
        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            var ext = System.IO.Path.GetExtension(e.FullPath).ToLower();
            if (!AppContext.TargetFileExtensionList.Contains(ext))
            {
                return;
            }

            lock (this)
            {
                if (!File.Exists(e.FullPath))
                {
                    return;
                }

                if (this.IsFileLocked(e.FullPath) == false)
                {
                    if (this.createdWatchList.Contains(e.FullPath))
                    {
                        this.createdWatchList.Remove(e.FullPath);
                    }

                    if (!AppContext.CheckFileSize(new FileInfo(e.FullPath).Length))
                    {
                        return;
                    }

                    this.OnCreated(e.FullPath);
                }
                else
                {
                    if (this.createdWatchList.Contains(e.FullPath) == false)
                    {
                        this.createdWatchList.Add(e.FullPath);
                    }
                }
            }
        }

        private void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            this.OnDeleted(e.FullPath);
        }

        private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            var ext = System.IO.Path.GetExtension(e.FullPath).ToLower();
            if (!AppContext.TargetFileExtensionList.Contains(ext))
            {
                return;
            }

            lock (this)
            {
                if (!File.Exists(e.FullPath))
                {
                    return;
                }

                if (this.IsFileLocked(e.FullPath) == false)
                {
                    if (this.changedWatchList.Contains(e.FullPath))
                    {
                        this.changedWatchList.Remove(e.FullPath);
                    }

                    if (!AppContext.CheckFileSize(new FileInfo(e.FullPath).Length))
                    {
                        return;
                    }

                    this.OnChanged(e.FullPath);
                }
                else
                {
                    if (this.changedWatchList.Contains(e.FullPath) == false)
                    {
                        this.changedWatchList.Add(e.FullPath);
                    }
                }
            }
        }

        private void FileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            this.OnDeleted(e.OldFullPath);
            this.OnCreated(e.FullPath);
        }

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        /// <param name="path">監視ディレクトリのパス。デフォルトはカレントディレクトリ。</param>
        /// <param name="filter">監視する対象のフィルタ条件。デフォルトはすべてのファイル「*」。</param>
        /// <param name="includeSubdirectories">サブディレクトリを検索するかどうかデフォルトは true。</param>
        /// <param name="interval">監視する間隔(msec)。デフォルトは 1000 ミリ秒。</param>
        /// <param name="synchronizingObject">同期</param>
        private void InitializeInstance(string path, string filter, bool? includeSubdirectories, double? interval, ISynchronizeInvoke synchronizingObject)
        {
            // インスタンス生成
            this.watcher = new FileSystemWatcher();
            this.timer = new Timer();

            // 初期設定
            this.Path = string.IsNullOrEmpty(path) ? Environment.CurrentDirectory : path;
            this.Filter = string.IsNullOrEmpty(filter) ? "*" : filter;
            this.IncludeSubdirectories = includeSubdirectories.GetValueOrDefault(true);
            this.Interval = interval.GetValueOrDefault(1000);

            if (synchronizingObject != null)
            {
                this.watcher.SynchronizingObject = synchronizingObject;
            }

            // イベントのアタッチ
            this.watcher.Created += new FileSystemEventHandler(this.FileSystemWatcher_Created);
            this.watcher.Deleted += new FileSystemEventHandler(this.FileSystemWatcher_Deleted);
            this.watcher.Changed += new FileSystemEventHandler(this.FileSystemWatcher_Changed);
            this.watcher.Renamed += new RenamedEventHandler(this.FileSystemWatcher_Renamed);
            this.timer.Elapsed += new ElapsedEventHandler(this.Timer_Elapsed);
        }

        /// <summary>
        /// 指定されたファイルがロックされているかどうかを返します。
        /// </summary>
        /// <param name="path">検証したいファイルへのフルパス</param>
        /// <returns>ロックされているかどうか</returns>
        private bool IsFileLocked(string path)
        {
            FileStream stream = null;

            try
            {
                stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch
            {
                return true;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

            return false;
        }

        /// <summary>
        /// Fireイベントを発生させます。
        /// </summary>
        /// <param name="fullpath">イベント発生源となったファイルへのフルパス</param>
        private void OnCreated(string fullpath)
        {
            if (this.FileAdded != null)
            {
                this.FileAdded(this, new FileCreationEventArgs(fullpath));
            }
        }

        private void OnDeleted(string fullpath)
        {
            if (this.FileRemoved != null)
            {
                this.FileRemoved(this, new FileRemovedEventArgs(fullpath));
            }
        }

        private void OnChanged(string fullpath)
        {
            if (this.FileUpdated != null)
            {
                this.FileUpdated(this, new FileUpdatedEventArgs(fullpath));
            }
        }

        /// <summary>
        /// TimerでElapsedイベントが発生したとき実行されます。
        /// </summary>
        /// <param name="sender">呼び出し元インスタンス</param>
        /// <param name="e">イベント引数</param>
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (this)
            {
                {
                    var toBeRemovedList = new List<string>();
                    foreach (var fullpath in this.createdWatchList)
                    {
                        if (this.IsFileLocked(fullpath) == false)
                        {
                            toBeRemovedList.Add(fullpath);
                            this.OnCreated(fullpath);
                        }
                    }

                    foreach (var fullPath in toBeRemovedList)
                    {
                        this.createdWatchList.Remove(fullPath);
                    }
                }

                {
                    var toBeRemovedList = new List<string>();
                    foreach (var fullpath in this.deletedWatchList)
                    {
                        if (this.IsFileLocked(fullpath) == false)
                        {
                            toBeRemovedList.Add(fullpath);
                            this.OnDeleted(fullpath);
                        }
                    }

                    foreach (var fullPath in toBeRemovedList)
                    {
                        this.deletedWatchList.Remove(fullPath);
                    }
                }

                {
                    var toBeRemovedList = new List<string>();
                    foreach (var fullpath in this.changedWatchList)
                    {
                        if (this.IsFileLocked(fullpath) == false)
                        {
                            toBeRemovedList.Remove(fullpath);
                            this.OnChanged(fullpath);
                        }
                    }

                    foreach (var fullPath in toBeRemovedList)
                    {
                        this.changedWatchList.Remove(fullPath);
                    }
                }
            }
        }
    }
}