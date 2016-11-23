using Ginger.Client.Exceptions;
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
    public class ServerStatusService
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string GetServerStatus(string serverUrl)
        {
            var service = new RestService(serverUrl);
            var response = service.Get(string.Empty);
            this.HandleException(response);
            var content = response.Content;
            var map = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content);
            var version = map["version"];
            return version;
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
