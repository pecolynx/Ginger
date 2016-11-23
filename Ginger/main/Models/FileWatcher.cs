using Ginger.Config;
using Ginger.LocalDataBase.Config;
using Ginger.LocalDataBase.Models;
using Ginger.LocalDataBase.Services;
using Ginger.Models.Io;
using Ginger.Services;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.Models
{
    public class FileWatcher
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private FileWatchTrigger fileWatchTrigger;

        public FileWatcher()
        {
            this.fileWatchTrigger = new FileWatchTrigger(AppContext.TargetDirectoryPath, "*", true, null);
            this.fileWatchTrigger.FileAdded += new EventHandler<FileCreationEventArgs>(this.FileWatch_Added);
            this.fileWatchTrigger.FileRemoved += new EventHandler<FileRemovedEventArgs>(this.FileWatch_Removed);
            this.fileWatchTrigger.FileUpdated += new EventHandler<FileUpdatedEventArgs>(this.FileWatch_Updated);
        }

        public AddFileJobToDataBase AddFileJobToDataBase { get; set; }

        public SynchronizeDocumentFile SynchronizeDocumentFile { get; set; }

        public GetFileJobList GetFileJobList { get; set; }

        public RemoveAllFileJob RemoveAllFileJob { get; set; }

        private string ServerUrl
        {
            get { return AppContext.ServerUrl; }
        }

        private string LoginId
        {
            get { return AppContext.LoginId; }
        }

        private string AuthToken
        {
            get { return AppContext.AuthToken; }
        }

        public void Start()
        {
            try
            {
                this.fileWatchTrigger.Start();
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.Message);
            }
        }

        public void Stop()
        {
            this.fileWatchTrigger.Stop();
        }

        private void FileWatch_Added(object sender, FileCreationEventArgs e)
        {
            this.AddFile(e.FullPath);
        }

        private void FileWatch_Removed(object sender, FileRemovedEventArgs e)
        {
            this.RemoveFile(e.FullPath);
        }

        private void FileWatch_Updated(object sender, FileUpdatedEventArgs e)
        {
            this.UpdateFile(e.FullPath);
        }

        private void AddFile(string filePath)
        {
            Logger.Warn("Added : " + filePath);

            //this.UpdateProgressAsync("ファイルが追加されました。同期を開始します。ファイル : " + filePath);

            this.AddFileJobToDataBase.Execute(filePath, LocalDataBaseConstants.COMMAND_ADD);

            this.Synchro();

            //this.UpdateProgressAsync("同期が完了しました。");
        }

        private void UpdateFile(string filePath)
        {
            Logger.Warn("Updated : " + filePath);

            //this.UpdateProgressAsync("ファイルが更新されました。同期を開始します。ファイル : " + filePath);

            this.AddFileJobToDataBase.Execute(filePath, LocalDataBaseConstants.COMMAND_UPDATE);

            this.Synchro();

            //this.UpdateProgressAsync("同期が完了しました。");
        }

        private void RemoveFile(string filePath)
        {
            Logger.Warn("Removed : " + filePath);

            //this.UpdateProgressAsync("ファイルが削除されました。同期を開始します。ファイル : " + filePath);

            this.AddFileJobToDataBase.Execute(filePath, LocalDataBaseConstants.COMMAND_REMOVE);
            this.Synchro();
            //this.UpdateProgressAsync("同期が完了しました。");
        }

        private void Synchro()
        {
            Logger.Warn("Synchro [ Begin ] ");
            var fileJobList = this.GetFileJobList.Execute();
            this.RemoveAllFileJob.Execute();
            var totalFileLength = fileJobList.Count;
            Logger.Warn("file job length : " + fileJobList.Count);
            if (totalFileLength == 0)
            {
                return;
            }

            foreach (var x in fileJobList)
            {
                var command = this.GetCommandstring(x.Command);
                var message = "ファイルを同期します(" + command + ")。ファイル : " + x.FilePath;
                Logger.Warn(message);
                this.SynchronizeDocumentFile_Execute(x);
            }

            Logger.Warn("Synchro [ End ] ");
        }

        private void SynchronizeDocumentFile_Execute(DbFileJob fileJob)
        {
            switch (fileJob.Command)
            {
                case LocalDataBaseConstants.COMMAND_ADD:
                    this.SynchronizeDocumentFile.Add(this.ServerUrl, this.LoginId, this.AuthToken, fileJob);
                    break;
                case LocalDataBaseConstants.COMMAND_REMOVE:
                    this.SynchronizeDocumentFile.Remove(this.ServerUrl, this.LoginId, this.AuthToken, fileJob);
                    break;
                case LocalDataBaseConstants.COMMAND_UPDATE:
                    this.SynchronizeDocumentFile.Update(this.ServerUrl, this.LoginId, this.AuthToken, fileJob);
                    break;
                default:
                    var message = "invalid command : " + fileJob.Command;
                    Logger.Warn(message);
                    throw new Exception(message);
            }
        }

        private void ThrowExceptionIfNotNull(Exception ex)
        {
            if (ex != null)
            {
                throw ex;
            }
        }

        private string GetCommandstring(int commandType)
        {
            switch (commandType)
            {
                case LocalDataBaseConstants.COMMAND_ADD:
                    return "追加";
                case LocalDataBaseConstants.COMMAND_REMOVE:
                    return "削除";
                case LocalDataBaseConstants.COMMAND_UPDATE:
                    return "更新";
                default:
                    throw new Exception(string.Empty);
            }
        }
    }
}
