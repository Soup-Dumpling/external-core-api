using External.Product.Core.Exceptions;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace External.Product.Core.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient httpClient;

        public HttpService(HttpClient httpClient) 
        {
            this.httpClient = httpClient;
        }

        public async Task<T> GetAsync<T>(string url)
        {
            var getUserRequest = await httpClient.GetAsync(url);
            if (getUserRequest.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFoundException();
            }
            getUserRequest.EnsureSuccessStatusCode();
            var responseString = await getUserRequest.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<T>(responseString);
            return response;
        }

        public async Task<T> PostAsync<T>(string url, StringContent data)
        {
            var postUserRequest = await httpClient.PostAsync(url, data);
            postUserRequest.EnsureSuccessStatusCode();
            var responseString = await postUserRequest.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<T>(responseString);
            return response;
        }

        public async Task<T> PutAsync<T>(string url, StringContent data)
        {
            var putUserRequest = await httpClient.PutAsync(url, data);
            if (putUserRequest.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFoundException();
            }
            putUserRequest.EnsureSuccessStatusCode();
            var responseString = await putUserRequest.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<T>(responseString);
            return response;
        }

        public async Task<T> PatchAsync<T>(string url, StringContent data)
        {
            var patchUserRequest = await httpClient.PatchAsync(url, data);
            if (patchUserRequest.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFoundException();
            }
            patchUserRequest.EnsureSuccessStatusCode();
            var responseString = await patchUserRequest.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<T>(responseString);
            return response;
        }

        public async Task<T> DeleteAsync<T>(string url)
        {
            var deleteUserRequest = await httpClient.DeleteAsync(url);
            if (deleteUserRequest.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFoundException();
            }
            deleteUserRequest.EnsureSuccessStatusCode();
            var responseString = await deleteUserRequest.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<T>(responseString);
            return response;
        }
    }
}
