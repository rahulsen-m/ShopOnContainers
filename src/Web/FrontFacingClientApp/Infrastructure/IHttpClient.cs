using System.Net.Http;
using System.Threading.Tasks;

namespace ShoesOnContainer.Web.ClientApp.Infrastructure
{
    /// <summary>
    /// Helps to create API calls to the remote API end points
    /// </summary>
    public interface IHttpClient
    {
        Task<string> GetStringAsync(string uri);

        Task<HttpResponseMessage> PostAsync<T>(string uri, T item);

        Task<HttpResponseMessage> DeleteAsync(string uri);

        Task<HttpResponseMessage> PutAsync<T>(string uri, T item);
    }
}
