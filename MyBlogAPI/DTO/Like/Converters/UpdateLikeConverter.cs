using AutoMapper;
using DbAccess.Repositories.Comment;
using DbAccess.Repositories.Post;
using DbAccess.Repositories.User;

namespace MyBlogAPI.DTO.Like.Converters
{
    public class UpdateLikeConverter
        : ITypeConverter<UpdateLikeDto, DbAccess.Data.POCO.Like>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;

        public UpdateLikeConverter(ICommentRepository commentRepository, IPostRepository postRepository,
            IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
            _userRepository = userRepository;
        }

        public DbAccess.Data.POCO.Like Convert(UpdateLikeDto source, DbAccess.Data.POCO.Like destination,
            ResolutionContext context)
        {
            destination.Post = source.Post != null ? _postRepository.Get(source.Post.Value) : null;
            destination.Comment = source.Comment != null ? _commentRepository.Get(source.Comment.Value) : null;
            destination.User = _userRepository.Get(source.User);
            destination.LikeableType = source.LikeableType;
            return destination;
        }
    }
}
