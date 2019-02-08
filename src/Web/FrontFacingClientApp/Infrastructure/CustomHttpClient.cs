using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShoesOnContainer.Web.ClientApp.Infrastructure
{
    /// <summary>
    /// Helps to create API calls to the remote API end points
    /// </summary>
    public class CustomHttpClient : IHttpClient
    {
        public CustomHttpClient(ILogger<CustomHttpClient> logger)
        {

        }
        public Task<string> GetStringAsync(string uri)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> PostAsync<T>(string uri, T item)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> PutAsync<T>(string uri, T item)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> DeleteAsync(string uri)
        {
            throw new NotImplementedException();
        }
    }
}
