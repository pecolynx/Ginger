using Ginger.Client.Exceptions;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.Client.Services
{
    public class AuthService
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// ユーザーを登録します。
        /// </summary>
        /// <param name="serverUrl">サーバーUrl</param>
        /// <param name="loginId">ログインId</param>
        /// <param name="loginPassword">ログインパスワード</param>
        /// <param name="email">メールアドレス</param>
        /// <param name="name">名前</param>
        public void Register(string serverUrl, string loginId, string loginPassword, string email, string name)
        {
            var service = new RestService(serverUrl);
            var parameter = new { loginId = loginId, loginPassword = loginPassword, email = email, name = name };
            var response = service.Post("auth/register", parameter);

            if (response.ErrorException != null)
            {
                Logger.Warn(response.ErrorMessage);
                throw new Exception(response.ErrorMessage);
            }
            else if (response.StatusCode != HttpStatusCode.Created)
            {
                var content = response.Content;
                Logger.Warn(content);
                throw new AuthException(content);
            }
        }

        /// <summary>
        /// 認証して認証トークンを取得します。
        /// </summary>
        /// <param name="serverUrl">サーバーUrl</param>
        /// <param name="loginId">ログインId</param>
        /// <param name="loginPassword">ログインパスワード</param>
        /// <returns>認証トークン</returns>
        public string Authenticate(string serverUrl, string loginId, string loginPassword)
        {
            var service = new RestService(serverUrl);
            var parameter = new { loginId = loginId, loginPassword = loginPassword };
            var response = service.Post("auth/authenticate", parameter);

            if (response.ErrorException != null)
            {
                Logger.Warn(response.ErrorMessage);
                throw new AuthException(response.ErrorMessage);
            }
            else if (response.StatusCode != HttpStatusCode.OK)
            {
                var content = response.Content;
                Logger.Warn(content);
                throw new AuthException(content);
            }

            var map = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content);
            var authToken = map["auth_token"];
            return authToken;
        }
    }
}
