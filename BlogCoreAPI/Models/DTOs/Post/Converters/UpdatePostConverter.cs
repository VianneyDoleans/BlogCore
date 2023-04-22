using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DBAccess.Data.JoiningEntity;
using DBAccess.Repositories.Category;
using DBAccess.Repositories.User;

namespace BlogCoreAPI.Models.DTOs.Post.Converters
{
    /// <summary>
    /// AutoMapper converter used to enable the conversion of <see cref="UpdatePostConverter"/> to <see cref="Post"/>.
    /// </summary>
    public class UpdatePostConverter : ITypeConverter<UpdatePostDto, DBAccess.Data.Post>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdatePostConverter"/> class.
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="categoryRepository"></param>
        public UpdatePostConverter(IUserRepository userRepository, ICategoryRepository categoryRepository)
        {
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
        }

        /// <inheritdoc />
        public DBAccess.Data.Post Convert(UpdatePostDto source, DBAccess.Data.Post destination,
            ResolutionContext context)
        {
            destination.Author = _userRepository.Get(source.Author);
            destination.Category = _categoryRepository.Get(source.Category);
            destination.Content = source.Content;
            destination.Name = source.Name;
            destination.ThumbnailUrl = string.IsNullOrEmpty(source.ThumbnailUrl) ? null : source.ThumbnailUrl;
            if (source.Tags != null)
                destination.PostTags = source.Tags.Select(x => new PostTag()
                {
                    PostId = destination.Id,
                    TagId = x
                }).ToList();
            else
                destination.PostTags = new List<PostTag>();
            return destination;
        }
    }
}
