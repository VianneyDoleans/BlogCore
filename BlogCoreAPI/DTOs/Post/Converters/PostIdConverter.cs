﻿using AutoMapper;
using DBAccess.Data.POCO;
using DBAccess.Repositories.Post;

namespace BlogCoreAPI.DTOs.Post.Converters
{
    /// <summary>
    /// AutoMapper converter used to enable the conversion of <see cref="Post"/> to its resource Id.
    /// </summary>
    public class PostIdConverter : ITypeConverter<int, DBAccess.Data.POCO.Post>
    {
        private readonly IPostRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostIdConverter"/> class.
        /// </summary>
        /// <param name="repository"></param>
        public PostIdConverter(IPostRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc />
        public DBAccess.Data.POCO.Post Convert(int source, DBAccess.Data.POCO.Post destination, ResolutionContext context)
        {
            return _repository.Get(source);
        }
    }
}
