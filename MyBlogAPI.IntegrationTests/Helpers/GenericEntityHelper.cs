using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MyBlogAPI.DTO;
using Newtonsoft.Json;

namespace MyBlogAPI.IntegrationTests.Helpers
{
    public class GenericEntityHelper<TGet, TAdd> where TGet : IDto, new()
        where TAdd : IDto, new()
    {
        private readonly string _baseUrl;
        private readonly HttpClient _client;

        public GenericEntityHelper(string baseUrl, HttpClient client)
        {
            _baseUrl = baseUrl;
            _client = client;
        }

        protected virtual TAdd CreateAddEntity()
        {
            var entity = new TAdd();
            return entity;
        }

        public async Task AddEntity(TAdd entity)
        {
            var json = JsonConvert.SerializeObject(entity);
            var httpResponse =
                await _client.PostAsync(_baseUrl, new StringContent(json, Encoding.UTF8, "application/json"));
            httpResponse.EnsureSuccessStatusCode();
        }

        public async Task<TAdd> AddRandomEntity()
        {
            var entity = CreateAddEntity();
            var json = JsonConvert.SerializeObject(entity);
            var httpResponse =
                await _client.PostAsync(_baseUrl, new StringContent(json, Encoding.UTF8, "application/json"));
            httpResponse.EnsureSuccessStatusCode();
            return entity;
        }

        public async Task<TGet> GetById(int id)
        {
            var httpGetResponse = await _client.GetAsync(_baseUrl + "/" + id);
            httpGetResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpGetResponse.Content.ReadAsStringAsync();
            var entity = JsonConvert.DeserializeObject<TGet>(stringResponse);
            return entity;
        }

        public async Task<IEnumerable<TGet>> GetAll()
        {
            var httpGetResponse = await _client.GetAsync(_baseUrl);
            httpGetResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpGetResponse.Content.ReadAsStringAsync();
            var entity = JsonConvert.DeserializeObject<IEnumerable<TGet>>(stringResponse);
            return entity;
        }

        public async Task UpdateIdentity(TAdd entity)
        {
            var json = JsonConvert.SerializeObject(entity);
            var httpResponse =
                await _client.PutAsync(_baseUrl, new StringContent(json, Encoding.UTF8, "application/json"));
            httpResponse.EnsureSuccessStatusCode();
        }

        public async Task RemoveIdentity(int id)
        {
            var httpResponse =
                await _client.DeleteAsync(_baseUrl + "/" + id);
            httpResponse.EnsureSuccessStatusCode();
        }
    }
}
