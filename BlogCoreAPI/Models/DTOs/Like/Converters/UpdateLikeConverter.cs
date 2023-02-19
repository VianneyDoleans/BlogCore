using AutoMapper;
using DBAccess.Repositories.Comment;
using DBAccess.Repositories.Post;
using DBAccess.Repositories.User;

namespace BlogCoreAPI.Models.DTOs.Like.Converters
{
    /// <summary>
    /// AutoMapper converter used to enable the conversion of <see cref="UpdateLikeDto"/> to <see cref="Like"/>.
    /// </summary>
    public class UpdateLikeConverter
        : ITypeConverter<UpdateLikeDto, DBAccess.Data.Like>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateLikeConverter"/> class.
        /// </summary>
        /// <param name="commentRepository"></param>
        /// <param name="postRepository"></param>
        /// <param name="userRepository"></param>
        public UpdateLikeConverter(ICommentRepository commentRepository, IPostRepository postRepository,
            IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
            _userRepository = userRepository;
        }

        /// <inheritdoc />
        public DBAccess.Data.Like Convert(UpdateLikeDto source, DBAccess.Data.Like destination,
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
