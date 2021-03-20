using System.Linq;
using AutoMapper;
using DbAccess.Data.POCO.JoiningEntity;
using DbAccess.Repositories.Category;
using DbAccess.Repositories.User;

namespace MyBlogAPI.DTO.Post.Converters
{
    public class UpdatePostConverter : ITypeConverter<UpdatePostDto, DbAccess.Data.POCO.Post>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;

        public UpdatePostConverter(IUserRepository userRepository, ICategoryRepository categoryRepository)
        {
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
        }

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
