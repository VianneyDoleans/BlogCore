using System.Collections.Generic;
using System.Threading.Tasks;
using BlogCoreAPI.Models.DTOs;

namespace BlogCoreAPI.FunctionalTests.Helpers
{
    public interface IEntityHelper<TGet, TAdd, TUpdate>
            where TGet : ADto, new()
            where TUpdate : ADto, new()
            where TAdd : new()
    {
        public abstract TUpdate GenerateTUpdate(int id, TGet entity);
        
        public Task<TGet> AddEntity(TAdd entity);

        // public Task UpdateRandomEntity(TUpdate entity);

        public Task<TGet> GetById(int id);

        public Task<IEnumerable<TGet>> GetAll();

        public Task UpdateEntity(TUpdate entity);

        public Task RemoveIdentity(int id);

        public bool Equals(TGet first, TGet second);
        
        public bool Equals(TUpdate first, TGet second);
        
        public bool Equals(TUpdate first, TUpdate second);
    }
}
