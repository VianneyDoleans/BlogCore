using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MyBlogAPI.DTO;
using MyBlogAPI.Responses;
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

        public abstract bool Equals(TGet first, TGet second);
        protected abstract TUpdate ModifyTUpdate(TUpdate entity);

        public async Task<TGet> GetById(int id)
        {
            var httpGetResponse = await _client.GetAsync(_baseUrl + "/" + id);
            httpGetResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpGetResponse.Content.ReadAsStringAsync();
            var entity = JsonConvert.DeserializeObject<TGet>(stringResponse);
            return entity;
        }

        private async Task<PagedBlogResponse<TGet>> GetEntities(int page, int limit)
        {
            var httpGetResponse = await _client.GetAsync(_baseUrl + "?size=" + limit + "&page=" + page);
            httpGetResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpGetResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<PagedBlogResponse<TGet>>(stringResponse);
            return result;
        }

        public async Task<IEnumerable<TGet>> GetAll()
        {
            var entities = new List<TGet>();
            const int limit = 10;

            var result = await GetEntities(1, limit);
            entities.AddRange(result.Data);
            for (var x = 2; (x - 1) * limit < result.Total; x += 1)
            {
                result = await GetEntities(x, limit);
                entities.AddRange(result.Data);
            }

            if (entities.Count != result.Total)
                throw new Exception("Error inside getAll, there are too many entities or not enough.");
            return entities;
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
