using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MyBlogAPI.DTO;
using Newtonsoft.Json;

namespace MyBlogAPI.IntegrationTests.Helpers
{
    public interface IEntityHelper<TGet, TAdd, TUpdate>
            where TGet : ADto, new()
            where TUpdate : ADto, new()
            where TAdd : new()
    {
        public Task<TGet> AddEntity(TAdd entity);

        public Task UpdateRandomEntity(TUpdate entity);

        public Task<TGet> AddRandomEntity();

        public Task<TGet> GetById(int id);

        public Task<IEnumerable<TGet>> GetAll();

        public Task UpdateIdentity(TUpdate entity);

        public Task RemoveIdentity(int id);

        public bool Equals(TGet first, TGet second);
    }
}
