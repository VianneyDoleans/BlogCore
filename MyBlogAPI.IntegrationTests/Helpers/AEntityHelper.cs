using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MyBlogAPI.DTO;
using Newtonsoft.Json;

namespace MyBlogAPI.IntegrationTests.Helpers
{
    public abstract class AEntityHelper<TGet, TAdd, TUpdate> : IEntityHelper<TGet, TAdd, TUpdate>
        where TGet : ADto, new()
        where TUpdate : ADto, new()
        where TAdd : new()
    {
        private readonly string _baseUrl;
        private readonly HttpClient _client;

        protected AEntityHelper(string baseUrl, HttpClient client)
        {
            _baseUrl = baseUrl;
            _client = client;
        }

        public async Task<TGet> AddEntity(TAdd entity)
        {
            var json = JsonConvert.SerializeObject(entity);
            var httpResponse =
                await _client.PostAsync(_baseUrl, new StringContent(json, Encoding.UTF8, "application/json"));
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TGet>(stringResponse);
        }

        protected abstract TAdd CreateTAdd();
        public abstract bool Equals(TGet first, TGet second);
        protected abstract TUpdate ModifyTUpdate(TUpdate entity);

        public async Task<TGet> AddRandomEntity()
        {
            var entity = CreateTAdd();
            var json = JsonConvert.SerializeObject(entity);
            var httpResponse =
                await _client.PostAsync(_baseUrl, new StringContent(json, Encoding.UTF8, "application/json"));
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TGet>(stringResponse);
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

        public async Task UpdateRandomEntity(TUpdate entity)
        {
            var entityModified = ModifyTUpdate(entity);
            await UpdateIdentity(entityModified);
        }

        public async Task UpdateIdentity(TUpdate entity)
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
