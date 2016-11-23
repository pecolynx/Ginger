using Ginger.Client.Exceptions;
using Ginger.Client.Models;
using Ginger.Common.Models;
using log4net;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.Client.Services
{
    public class DocumentFileService
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ModelId AddDocumentFile(string serverUrl, string loginId, string authToken, DocumentFile documentFile)
        {
            System.Diagnostics.Debug.WriteLine("updated " + documentFile.FileUpdatedAt.ToString());
            var service = new RestService(serverUrl);
            var urlParameter = this.CreateUrlParameter(authToken);
            var parameter = new
            {
                documentFile = new
                {
                    fileName = documentFile.FileName,
                    filePath = documentFile.FilePath,
                    fileContent = documentFile.FileContent,
                    fileHash = documentFile.FileHash,
                    fileCreatedAt = documentFile.FileCreatedAt.ToString(),
                    fileUpdatedAt = documentFile.FileUpdatedAt.ToString()
                }
            };

            Logger.Info("fileContent length : " + documentFile.FileContent.Length);

            var response = service.Post("document/file?authToken={authToken}", urlParameter, parameter);
            this.HandleException(response);

            var content = response.Content;

            Logger.Info(content);

            var map = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content);
            var modelId = new ModelId(map["documentId"]);
            return modelId;
        }

        public void RemoveDocumentFile(string serverUrl, string loginId, string authToken, ModelId documentId)
        {
            var service = new RestService(serverUrl);
            var urlParameter = this.CreateUrlParameter(authToken, documentId);

            var response = service.Delete("document/file/{id}?authToken={authToken}", urlParameter);
            this.HandleException(response);
        }

        public void UpdateDocumentFile(string serverUrl, string loginId, string authToken, DocumentFile documentFile)
        {
            var service = new RestService(serverUrl);
            var urlParameter = this.CreateUrlParameter(authToken, new ModelId(documentFile.Id));

            var parameter = new
            {
                documentFile = new
                {
                    id = documentFile.Id,
                    fileName = documentFile.FileName,
                    filePath = documentFile.FilePath,
                    fileContent = documentFile.FileContent,
                    fileHash = documentFile.FileHash,
                    fileCreatedAt = documentFile.FileCreatedAt.ToString(),
                    fileUpdatedAt = documentFile.FileUpdatedAt.ToString()
                }
            };

            Logger.Info("fileContent length : " + documentFile.FileContent.Length);

            var response = service.Put("document/file/{id}?authToken={authToken}", urlParameter, parameter);
            this.HandleException(response);
        }

        public void ClearDocument(string serverUrl, string loginId, string authToken)
        {
            var service = new RestService(serverUrl);
            var urlParameter = this.CreateUrlParameter(authToken);
            var response = service.Delete("document/file/clear?authToken={authToken}", urlParameter);
            this.HandleException(response);
        }

        public SearchResult SearchFile(string serverUrl, string authToken, List<string> keywordList, List<string> extList, int pageNumber, int pageSize)
        {
            var service = new RestService(serverUrl);
            var urlParameter = this.CreateUrlParameter(authToken);
            var parameter = new
            {
                searchCondition = new
                {
                    pageNumber = pageNumber,
                    pageSize = pageSize,
                    keywordList = keywordList,
                    extList = extList
                }
            };

            var response = service.Post("document/file/search?authToken={authToken}", urlParameter, parameter);
            this.HandleException(response);

            var searchResult = JsonConvert.DeserializeObject<SearchResult>(response.Content);
            System.Diagnostics.Debug.WriteLine(response.Content);
            return searchResult;
        }

        public List<string> GetFilePathList(string serverUrl, string authToken, int pageNumber)
        {
            var service = new RestService(serverUrl);
            var urlParameter = this.CreateUrlParameter(authToken);
            var parameter = new
            {
                page = new
                {
                    pageNumber = pageNumber,
                    pageSize = 1000
                }
            };

            var response = service.Post("document/file/list?authToken={authToken}", urlParameter, parameter);
            this.HandleException(response);

            Logger.Info("document/file/list = " + response.Content);

            return JsonConvert.DeserializeObject<List<string>>(response.Content);
        }

        private Dictionary<string, string> CreateUrlParameter(string authToken)
        {
            return new Dictionary<string, string>()
            {
                { "authToken", authToken }
            };
        }

        private Dictionary<string, string> CreateUrlParameter(string authToken, ModelId documentId)
        {
            return new Dictionary<string, string>()
            {
                { "id", documentId.Value },
                { "authToken", authToken }
            };
        }

        private void HandleException(IRestResponse response)
        {
            if (response.StatusCode == 0)
            {
                Logger.Warn(response.ErrorMessage);
                throw new ConnectionException(response.ErrorMessage);
            }
            else if (response.ErrorException != null)
            {
                Logger.Warn(response.ErrorMessage);
                throw new ApplicationException(response.ErrorMessage);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                Logger.Warn(response.Content);
                throw new AuthException(response.Content);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                Logger.Warn(response.Content);
                throw new BadRequestException(response.Content);
            }
            else if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Logger.Warn(response.Content);
                throw new ApplicationException(response.ErrorMessage);
            }
        }
    }
}
