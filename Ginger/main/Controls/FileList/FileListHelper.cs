using Ginger.Client.Services;
using Ginger.Config;
using Ginger.LocalDataBase.Config;
using Ginger.LocalDataBase.Models;
using Ginger.LocalDataBase.Services;
using Ginger.LocalDataBase.Services.LocalFile;
using Ginger.LocalDataBase.Services.ServerFile;
using Ginger.Models.Io;
using Ginger.Services;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.Controls.FileList
{
    internal class FileListHelper
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SynchronizeDocumentFile SynchronizeDocumentFile { get; set; }

        public DocumentFileService DocumentFileService { get; set; }

        public ClearDb ClearDb { get; set; }

        public MigrateDataBase MigrateDataBase { get; set; }

        public AddLocalFile AddLocalFile { get; set; }

        public AddServerFile AddServerFile { get; set; }

        public GetFileListToBeAdded GetFileListToBeAdded { get; set; }

        public GetFileListToBeRemoved GetFileListToBeRemoved { get; set; }

        public GetFileListToBeUpdated GetFileListToBeUpdated { get; set; }

        public GetFileInfoListToBeUpdated GetFileInfoListToBeUpdated { get; set; }

        public AddFileJobToDataBase AddFileJobToDataBase { get; set; }

        public RemoveAllLocalFile RemoveAllLocalFile { get; set; }

        public RemoveAllServerFile RemoveAllServerFile { get; set; }

        public GetLocalFileList GetLocalFileList { get; set; }

        public GetServerFileList GetServerFileList { get; set; }

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

        public Exception RemoveAllLocalFile_Execute()
        {
            try
            {
                this.RemoveAllLocalFile.Execute();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public Exception RemoveAllServerFile_Execute()
        {
            try
            {
                this.RemoveAllServerFile.Execute();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public Exception GetFileListToBeAdded_Execute(out List<string> list)
        {
            try
            {
                list = this.GetFileListToBeAdded.Execute();
                return null;
            }
            catch (Exception ex)
            {
                list = null;
                return ex;
            }
        }

        public Exception GetFileListToBeRemoved_Execute(out List<string> list)
        {
            list = null;
            try
            {
                list = this.GetFileListToBeRemoved.Execute();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public Exception GetFileListToBeUpdated_Execute(out List<string> list)
        {
            list = null;
            try
            {
                list = this.GetFileListToBeUpdated.Execute();
                list.ForEach(x => Logger.Warn("ToBeUpdated : " + x));
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public Exception GetFileListToBeUpdatedForDiff_Execute(out List<FileDiff> list)
        {
            list = null;
            try
            {
                list = this.GetFileInfoListToBeUpdated.Execute();
                list.ForEach(x => Logger.Warn("ToBeUpdated : " + x));
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public Exception AddFileJobToDataBase_Execute(string filePath, int command)
        {
            try
            {
                this.AddFileJobToDataBase.Execute(filePath, command);
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public Exception SynchronizeDocumentFile_Execute(DbFileJob fileJob)
        {
            try
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

                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public Exception DocumentFileService_ClearDocument()
        {
            try
            {
                this.DocumentFileService.ClearDocument(this.ServerUrl, this.LoginId, this.AuthToken);
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public Exception ClearDb_Execute()
        {
            try
            {
                this.ClearDb.Execute();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public Exception MigrateDataBase_Execute()
        {
            try
            {
                this.MigrateDataBase.Execute();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public Exception AddLocalFile_Execute(string directoryPath)
        {
            try
            {
                foreach (var filePath in new DirectoryReader().ReadFileList(directoryPath).Where(this.ContainsTargetExtension))
                {
                    this.AddLocalFile.Execute(filePath, File.GetCreationTime(filePath), File.GetLastWriteTime(filePath));
                }

                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public Exception AddServerFile_Execute(string serverUrl, string authToken)
        {
            try
            {
                var pageNumber = 1;
                while (true)
                {
                    var filePathList = this.DocumentFileService.GetFilePathList(serverUrl, authToken, pageNumber);
                    if (filePathList.Count() == 0)
                    {
                        break;
                    }

                    foreach (var filePath in filePathList)
                    {
                        this.AddServerFile.Execute(filePath);
                    }

                    pageNumber++;
                }

                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public Exception GetLocalFileList_Execute(out List<string> list)
        {
            try
            {
                list = this.GetLocalFileList.Execute();
                return null;
            }
            catch (Exception ex)
            {
                list = null;
                return ex;
            }
        }

        public Exception GetServerFileList_Execute(out List<string> list)
        {
            try
            {
                list = this.GetServerFileList.Execute();
                return null;
            }
            catch (Exception ex)
            {
                list = null;
                return ex;
            }
        }

        public string GetCommandstring(int commandType)
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

        public bool ContainsTargetExtension(string filePath)
        {
            var ext = System.IO.Path.GetExtension(filePath).ToLower();
            if (!AppContext.TargetFileExtensionList.Contains(ext))
            {
                return false;
            }

            return true;
        }
    }
}
