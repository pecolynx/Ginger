using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.Client.Services
{
    /// <summary>
    /// Restサービス
    /// </summary>
    public class RestService
    {
        private RestClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestService"/> class.
        /// コンストラクタ
        /// </summary>
        /// <param name="url">基本Url</param>
        public RestService(string url)
        {
            this.client = new RestClient(url);
        }

        /// <summary>
        /// Getメソッドの呼び出し
        /// </summary>
        /// <param name="resource">Url</param>
        /// <returns>レスポンス</returns>
        public IRestResponse Get(string resource)
        {
            var request = this.CreateJsonRequest(resource, Method.GET);
            return this.client.Execute(request);
        }

        /// <summary>
        /// Postメソッドの呼び出し
        /// </summary>
        /// <param name="resource">Url</param>
        /// <param name="parameter">Bodyパラメータ</param>
        /// <returns>レスポンス</returns>
        public IRestResponse Post(string resource, object parameter)
        {
            var request = this.CreateJsonRequest(resource, Method.POST);
            request.AddBody(parameter);
            return this.client.Execute(request);
        }

        /// <summary>
        /// Postメソッドの呼び出し
        /// </summary>
        /// <param name="resource">Url</param>
        /// <param name="urlParameter">Urlパラメータ</param>
        /// <param name="parameter">Bodyパラメータ</param>
        /// <returns>レスポンス</returns>
        public IRestResponse Post(string resource, IDictionary<string, string> urlParameter, object parameter)
        {
            var request = this.CreateJsonRequest(resource, Method.POST);
            foreach (var x in urlParameter)
            {
                request.AddUrlSegment(x.Key, x.Value);
            }

            request.AddBody(parameter);
            return this.client.Execute(request);
        }

        /// <summary>
        /// Putメソッドの呼び出し
        /// </summary>
        /// <param name="resource">Url</param>
        /// <param name="urlParameter">Urlパラメータ</param>
        /// <param name="parameter">Bodyパラメータ</param>
        /// <returns>レスポンス</returns>
        public IRestResponse Put(string resource, IDictionary<string, string> urlParameter, object parameter)
        {
            var request = this.CreateJsonRequest(resource, Method.PUT);
            foreach (var x in urlParameter)
            {
                request.AddUrlSegment(x.Key, x.Value);
            }

            request.AddBody(parameter);
            return this.client.Execute(request);
        }

        /// <summary>
        /// Deleteメソッドの呼び出し
        /// </summary>
        /// <param name="resource">Url</param>
        /// <returns>レスポンス</returns>
        public IRestResponse Delete(string resource)
        {
            var request = this.CreateJsonRequest(resource, Method.DELETE);
            return this.client.Execute(request);
        }

        /// <summary>
        /// Deleteメソッドの呼び出し
        /// </summary>
        /// <param name="resource">Url</param>
        /// <param name="urlParameter">Urlパラメータ</param>
        /// <returns>レスポンス</returns>
        public IRestResponse Delete(string resource, IDictionary<string, string> urlParameter)
        {
            var request = this.CreateJsonRequest(resource, Method.DELETE);
            foreach (var x in urlParameter)
            {
                request.AddUrlSegment(x.Key, x.Value);
            }

            return this.client.Execute(request);
        }

        private RestRequest CreateJsonRequest(string resource, Method method)
        {
            var request = new RestRequest(resource, method);
            request.RequestFormat = DataFormat.Json;
            return request;
        }
    }
}
