﻿using AutoMapper;
using DBAccess.Data.POCO;
using DBAccess.Repositories.Category;

namespace BlogCoreAPI.DTOs.Category.Converters
{
    /// <summary>
    /// AutoMapper converter used to enable the conversion of <see cref="Category"/> to its resource Id.
    /// </summary>
    public class CategoryIdConverter : ITypeConverter<int, DBAccess.Data.POCO.Category>
    {
        private readonly ICategoryRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryIdConverter"/> class.
        /// </summary>
        /// <param name="repository"></param>
        public CategoryIdConverter(ICategoryRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc />
        public DBAccess.Data.POCO.Category Convert(int source, DBAccess.Data.POCO.Category destination, ResolutionContext context)
        {
            return _repository.Get(source);
        }
    }
}
