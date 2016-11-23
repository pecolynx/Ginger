using Ginger.Client.Exceptions;
using Ginger.Client.Models;
using Ginger.Client.Services;
using Ginger.Config;
using Ginger.LocalDataBase.Config;
using Ginger.LocalDataBase.Models;
using Ginger.LocalDataBase.Services;
using Ginger.LocalDataBase.Services.LocalFile;
using Ginger.LocalDataBase.Services.ServerFile;
using Ginger.Models;
using Ginger.Models.Io;
using Ginger.Services;
using log4net;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Ginger.Controls.FileList
{
    public class FileListViewModel
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private RemoveAllLocalFile removeAllLocalFile;

        private RemoveAllServerFile removeAllServerFile;

        private RemoveAllFileJob removeAllFileJob;

        private GetFileJobList getFileJobList;

        private FileListHelper fileListHelper;

        public FileListViewModel()
        {
            var isDesignMode = (bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue;
            if (isDesignMode)
            {
                return;
            }

            this.removeAllLocalFile = AppContext.GetObject<RemoveAllLocalFile>();
            this.removeAllServerFile = AppContext.GetObject<RemoveAllServerFile>();

            this.getFileJobList = AppContext.GetObject<GetFileJobList>();
            this.removeAllFileJob = AppContext.GetObject<RemoveAllFileJob>();

            this.fileListHelper = AppContext.GetObject<FileListHelper>();
        }

        public delegate void StringEventHandler(string value);

        public ReactiveProperty<List<LocalServerFileInfo>> FileInfoList { get; } = new ReactiveProperty<List<LocalServerFileInfo>>();

        public StringEventHandler UpdateProgress { private get; set; }

        public ReactiveProperty<Visibility> ProgressBarVisibility { get; } = new ReactiveProperty<Visibility>();

        public ReactiveProperty<int> ProgressValue { get; } = new ReactiveProperty<int>();

        public async void Search()
        {
            await Task.Run(() =>
            {
                this.removeAllLocalFile.Execute();
                this.removeAllServerFile.Execute();
            });

            this.UpdateProgress("ローカルフォルダを検査します。");
            this.ThrowExceptionIfNotNull(await Task.Run(() => this.fileListHelper.AddLocalFile_Execute(AppContext.TargetDirectoryPath)));

            this.UpdateProgress("サーバーからファイルパス情報を取得します。");
            this.ThrowExceptionIfNotNull(await Task.Run(() => this.fileListHelper.AddServerFile_Execute(AppContext.ServerUrl, AppContext.AuthToken)));

            List<string> toBeAdded = null;
            this.ThrowExceptionIfNotNull(await Task.Run(() => this.fileListHelper.GetFileListToBeAdded_Execute(out toBeAdded)));

            List<string> toBeRemoved = null;
            this.ThrowExceptionIfNotNull(await Task.Run(() => this.fileListHelper.GetFileListToBeRemoved_Execute(out toBeRemoved)));

            List<FileDiff> toBeUpdated = null;
            this.ThrowExceptionIfNotNull(await Task.Run(() => this.fileListHelper.GetFileListToBeUpdatedForDiff_Execute(out toBeUpdated)));

            this.UpdateProgress("ファイル情報を取得しました。");

            this.FileInfoList.Value = new List<LocalServerFileInfo>();
            foreach (var x in toBeAdded)
            {
                this.FileInfoList.Value.Add(new LocalServerFileInfo(x, string.Empty, string.Empty, string.Empty, "added"));
            }

            foreach (var x in toBeRemoved)
            {
                this.FileInfoList.Value.Add(new LocalServerFileInfo(x, string.Empty, string.Empty, string.Empty, "removed"));
            }

            foreach (var x in toBeUpdated)
            {
                this.FileInfoList.Value.Add(new LocalServerFileInfo(x.FilePath, string.Empty, x.DateTime1.ToString(), x.DateTime2.ToString(), "updated"));
            }
        }

        public async void ClearDocumentFile()
        {
            try
            {
                this.UpdateProgress("サーバーのファイルを初期化します。");
                this.ThrowExceptionIfNotNull(await Task.Run(() => this.fileListHelper.DocumentFileService_ClearDocument()));

                this.UpdateProgress("ローカルのデータベースを初期化します。");
                this.ThrowExceptionIfNotNull(await Task.Run(() => this.fileListHelper.ClearDb_Execute()));

                this.UpdateProgress("ローカルのデータベースを構築します。");
                this.ThrowExceptionIfNotNull(await Task.Run(() => this.fileListHelper.MigrateDataBase_Execute()));

                this.UpdateProgress("初期化が完了しました。");
            }
            catch (BadRequestException)
            {
                this.UpdateProgress("失敗しました。");
            }
            catch (AuthException)
            {
                this.UpdateProgress("認証に失敗しました。");
            }
            catch (Exception)
            {
                this.UpdateProgress("失敗しました。");
            }
        }

        public async void GetLocalFileList1()
        {
            await Task.Run(() =>
            {
                this.removeAllLocalFile.Execute();
                this.removeAllServerFile.Execute();
            });

            this.UpdateProgress("ローカルフォルダを検査します。");
            this.ThrowExceptionIfNotNull(await Task.Run(() => this.fileListHelper.AddLocalFile_Execute(AppContext.TargetDirectoryPath)));

            List<string> toBeAdded = null;
            this.ThrowExceptionIfNotNull(await Task.Run(() => this.fileListHelper.GetLocalFileList_Execute(out toBeAdded)));

            this.UpdateProgress("ファイル情報を取得しました。");

            this.FileInfoList.Value = new List<LocalServerFileInfo>();
            foreach (var x in toBeAdded)
            {
                this.FileInfoList.Value.Add(new LocalServerFileInfo(x, string.Empty, string.Empty, string.Empty, "local"));
            }
        }

        public async void GetServerFileList1()
        {
            await Task.Run(() =>
            {
                this.removeAllLocalFile.Execute();
                this.removeAllServerFile.Execute();
            });

            this.UpdateProgress("サーバーからファイルパス情報を取得します。");
            this.ThrowExceptionIfNotNull(await Task.Run(() => this.fileListHelper.AddServerFile_Execute(AppContext.ServerUrl, AppContext.AuthToken)));

            List<string> toBeAdded = null;
            this.ThrowExceptionIfNotNull(await Task.Run(() => this.fileListHelper.GetServerFileList_Execute(out toBeAdded)));

            this.UpdateProgress("ファイル情報を取得しました。");

            this.FileInfoList.Value = new List<LocalServerFileInfo>();
            foreach (var x in toBeAdded)
            {
                this.FileInfoList.Value.Add(new LocalServerFileInfo(x, string.Empty, string.Empty, string.Empty, "server"));
            }
        }

        internal async void SyncrhoAll()
        {
            try
            {
                this.ThrowExceptionIfNotNull(await Task.Run(() => this.fileListHelper.RemoveAllLocalFile_Execute()));
                this.ThrowExceptionIfNotNull(await Task.Run(() => this.fileListHelper.RemoveAllServerFile_Execute()));

                this.UpdateProgress("ローカルフォルダを検査します。");
                this.ThrowExceptionIfNotNull(await Task.Run(() => this.fileListHelper.AddLocalFile_Execute(AppContext.TargetDirectoryPath)));

                this.UpdateProgress("サーバーからファイルパス情報を取得します。");
                this.ThrowExceptionIfNotNull(await Task.Run(() => this.fileListHelper.AddServerFile_Execute(AppContext.ServerUrl, AppContext.AuthToken)));

                List<string> toBeAdded = null;
                this.ThrowExceptionIfNotNull(await Task.Run(() => this.fileListHelper.GetFileListToBeAdded_Execute(out toBeAdded)));

                List<string> toBeRemoved = null;
                this.ThrowExceptionIfNotNull(await Task.Run(() => this.fileListHelper.GetFileListToBeRemoved_Execute(out toBeRemoved)));

                List<string> toBeUpdated = null;
                this.ThrowExceptionIfNotNull(await Task.Run(() => this.fileListHelper.GetFileListToBeUpdated_Execute(out toBeUpdated)));

                foreach (var x in toBeAdded)
                {
                    this.UpdateProgress("同期対象リストに追加します。追加ファイル : " + x);
                    this.ThrowExceptionIfNotNull(await Task.Run(() => this.fileListHelper.AddFileJobToDataBase_Execute(x, LocalDataBaseConstants.COMMAND_ADD)));
                }

                foreach (var x in toBeRemoved)
                {
                    this.UpdateProgress("同期対象リストに追加します。削除ファイル : " + x);
                    this.ThrowExceptionIfNotNull(await Task.Run(() => this.fileListHelper.AddFileJobToDataBase_Execute(x, LocalDataBaseConstants.COMMAND_REMOVE)));
                }

                foreach (var x in toBeUpdated)
                {
                    this.UpdateProgress("同期対象リストに追加します。更新ファイル : " + x);
                    this.ThrowExceptionIfNotNull(await Task.Run(() => this.fileListHelper.AddFileJobToDataBase_Execute(x, LocalDataBaseConstants.COMMAND_UPDATE)));
                }

                var count = 0;
                var fileJobList = this.getFileJobList.Execute();
                this.removeAllFileJob.Execute();
                var totalFileLength = fileJobList.Count;
                Logger.Warn("file job length : " + fileJobList.Count);
                if (totalFileLength != 0)
                {
                    this.ProgressValue.Value = count * 100 / totalFileLength;
                    this.ProgressBarVisibility.Value = Visibility.Visible;
                    foreach (var x in fileJobList)
                    {
                        var command = this.fileListHelper.GetCommandstring(x.Command);
                        var message = "ファイルを同期します(" + command + ")。ファイル : " + x.FilePath;
                        this.UpdateProgress(message);
                        Logger.Warn(message);
                        this.ThrowExceptionIfNotNull(await Task.Run(() => this.fileListHelper.SynchronizeDocumentFile_Execute(x)));
                        count++;
                        this.ProgressValue.Value = count * 100 / totalFileLength;
                    }
                }

                this.UpdateProgress("同期が完了しました。");
            }
            catch (BadRequestException)
            {
                this.UpdateProgress("失敗しました。");
            }
            catch (AuthException)
            {
                this.UpdateProgress("認証に失敗しました。");
            }
            catch (Exception)
            {
                this.UpdateProgress("失敗しました。");
            }
            finally
            {
                this.ThrowExceptionIfNotNull(await Task.Run(() => this.fileListHelper.RemoveAllLocalFile_Execute()));
                this.ThrowExceptionIfNotNull(await Task.Run(() => this.fileListHelper.RemoveAllServerFile_Execute()));
            }
        }

        private bool ContainsTargetExtension(string filePath)
        {
            var ext = System.IO.Path.GetExtension(filePath).ToLower();
            if (!AppContext.TargetFileExtensionList.Contains(ext))
            {
                return false;
            }

            return true;
        }

        private void ThrowExceptionIfNotNull(Exception ex)
        {
            if (ex != null)
            {
                throw ex;
            }
        }
    }
}
