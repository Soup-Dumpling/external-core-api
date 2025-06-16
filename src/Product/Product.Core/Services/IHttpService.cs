using System.Net.Http;
using System.Threading.Tasks;

namespace External.Product.Core.Services
{
    public interface IHttpService
    {
        public Task<T> GetAsync<T>(string url);
        public Task<T> PostAsync<T>(string url, StringContent data);
        public Task<T> PutAsync<T>(string url, StringContent data);
        public Task<T> PatchAsync<T>(string url, StringContent data);
        public Task<T> DeleteAsync<T>(string url);
    }
}
