using System.Linq;
using AutoMapper;
using DbAccess.Data.POCO.JoiningEntity;
using DbAccess.Repositories.Category;
using DbAccess.Repositories.User;

namespace MyBlogAPI.DTO.Post.Converters
{
    /// <summary>
    /// AutoMapper converter used to enable the conversion of <see cref="UpdatePostConverter"/> to <see cref="DbAccess.Data.POCO.Post"/>.
    /// </summary>
    public class UpdatePostConverter : ITypeConverter<UpdatePostDto, DbAccess.Data.POCO.Post>
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
        public DbAccess.Data.POCO.Post Convert(UpdatePostDto source, DbAccess.Data.POCO.Post destination,
            ResolutionContext context)
        {
            destination.Author = _userRepository.Get(source.Author);
            destination.Category = _categoryRepository.Get(source.Category);
            destination.Content = source.Content;
            destination.Name = source.Name;
            if (source.Tags != null)
                destination.PostTags = source.Tags.Select(x => new PostTag()
                {
                    PostId = destination.Id,
                    TagId = x
                }).ToList();
            return destination;
        }
    }
}
