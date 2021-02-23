using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DbAccess.Repositories.Category;

namespace MyBlogAPI.DTO.Category
{
    public class CategoryDtoConverter : ITypeConverter<int, DbAccess.Data.POCO.Category>
    {
        private readonly ICategoryRepository _repository;

        public CategoryDtoConverter(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public DbAccess.Data.POCO.Category Convert(int source, DbAccess.Data.POCO.Category destination, ResolutionContext context)
        {
            return _repository.Get(source);
        }
    }
}
