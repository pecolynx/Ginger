using Ginger.Client.Models;
using Ginger.Client.Services;
using Ginger.Config;
using Ginger.LocalDataBase.Config;
using Ginger.LocalDataBase.Models;
using Ginger.LocalDataBase.Services;
using Ginger.Models;
using Ginger.Models.ExcelFile;
using Ginger.Models.Io.TextFile;
using Ginger.Models.WordFile;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Ginger.Services
{
    public class SynchronizeDocumentFile
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AddFileToDataBase AddFileToDataBase { get; set; }

        public GetFileJobList GetFileJobList { get; set; }

        public GetDocumentIdByFilePathFromDataBase GetDocumentIdByFilePathFromDataBase { get; set; }

        public UpdateFileToDataBase UpdateFileToDataBase { get; set; }

        public RemoveFileToDataBase RemoveFileToDataBase { get; set; }

        public DocumentFileService DocumentFileService { get; set; }

        public void Add(string serverUrl, string loginId, string authToken, DbFileJob dbFileJob)
        {
            lock (this)
            {
                var filePath = dbFileJob.FilePath;
                if (!File.Exists(filePath))
                {
                    return;
                }

                var fileReader = this.BuildFileReader(filePath);
                if (fileReader == null)
                {
                    return;
                }

                var fileContent = fileReader.Execute();

                DateTime fileCreatedAt = File.GetCreationTime(filePath);
                DateTime fileUpdatedAt = File.GetLastWriteTime(filePath);

                System.Diagnostics.Debug.WriteLine("*** updated " + fileUpdatedAt.ToString());

                var documentFile = new DocumentFile(Path.GetFileName(filePath), filePath, fileContent, string.Empty, fileCreatedAt, fileUpdatedAt);
                var modelId = this.DocumentFileService.AddDocumentFile(serverUrl, loginId, authToken, documentFile);
                this.AddFileToDataBase.Execute(filePath, modelId.Value, fileCreatedAt, fileUpdatedAt);
            }
        }

        public void Update(string serverUrl, string loginId, string authToken, DbFileJob dbFileJob)
        {
            lock (this)
            {
                var filePath = dbFileJob.FilePath;
                if (!File.Exists(filePath))
                {
                    return;
                }

                var fileReader = this.BuildFileReader(filePath);
                if (fileReader == null)
                {
                    return;
                }

                var modelId = this.GetDocumentIdByFilePathFromDataBase.Execute(filePath);
                if (modelId == null)
                {
                    return;
                }

                DateTime fileCreatedAt = File.GetCreationTime(filePath);
                DateTime fileUpdatedAt = File.GetLastWriteTime(filePath);

                var fileContent = fileReader.Execute();
                var documentFile = new DocumentFile(modelId.Value, Path.GetFileName(filePath), filePath, fileContent, string.Empty, fileCreatedAt, fileUpdatedAt);
                this.DocumentFileService.UpdateDocumentFile(serverUrl, loginId, authToken, documentFile);
                this.UpdateFileToDataBase.Execute(filePath, fileCreatedAt, fileUpdatedAt);
            }
        }

        public void Remove(string serverUrl, string loginId, string authToken, DbFileJob dbFileJob)
        {
            lock (this)
            {
                var filePath = dbFileJob.FilePath;
                var modelId = this.GetDocumentIdByFilePathFromDataBase.Execute(filePath);
                if (modelId == null)
                {
                    return;
                }

                this.DocumentFileService.RemoveDocumentFile(serverUrl, loginId, authToken, modelId);
                this.RemoveFileToDataBase.Execute(filePath);
            }
        }

        public IFileReaderBase BuildFileReader(string filePath)
        {
            var ext = Path.GetExtension(filePath).ToLower();
            switch (ext)
            {
                case ".txt":
                case ".csv":
                case ".md":
                    return new TextFileReader(filePath);
                case ".docx":
                    return new DocxReader(filePath);
                case ".xlsx":
                    return new XlsxReader(filePath);
                case ".xls":
                    return new XlsReader(filePath);
                default:
                    return null;
            }
        }
    }
}
