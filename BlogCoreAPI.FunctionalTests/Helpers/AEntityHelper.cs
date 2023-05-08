using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BlogCoreAPI.Models.DTOs;
using BlogCoreAPI.Responses;
using Newtonsoft.Json;

namespace BlogCoreAPI.FunctionalTests.Helpers
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

        public abstract bool Equals(TUpdate first, TGet second);
        
        public abstract bool Equals(TUpdate first, TUpdate second);
        
        public abstract bool Equals(TGet first, TGet second);
        
        public abstract TUpdate GenerateTUpdate(int id, TGet entity);

        public virtual async Task<TGet> AddEntity(TAdd entity)
        {
            var json = JsonConvert.SerializeObject(entity);
            var httpResponse =
                await _client.PostAsync(_baseUrl, new StringContent(json, Encoding.UTF8, "application/json"));
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TGet>(stringResponse);
        }

        public virtual async Task<TGet> GetById(int id)
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

        public virtual async Task UpdateEntity(TUpdate entity)
        {
            var json = JsonConvert.SerializeObject(entity);
            var httpResponse =
                await _client.PutAsync(_baseUrl, new StringContent(json, Encoding.UTF8, "application/json"));
            httpResponse.EnsureSuccessStatusCode();
        }

        public virtual async Task RemoveIdentity(int id)
        {
            var httpResponse =
                await _client.DeleteAsync(_baseUrl + "/" + id);
            httpResponse.EnsureSuccessStatusCode();
        }
    }
}
