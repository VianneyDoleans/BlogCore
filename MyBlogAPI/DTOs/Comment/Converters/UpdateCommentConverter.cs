using AutoMapper;
using DbAccess.Repositories.Comment;
using DbAccess.Repositories.Post;
using DbAccess.Repositories.User;

namespace MyBlogAPI.DTO.Comment.Converters
{
    /// <summary>
    /// AutoMapper converter used to enable the conversion of <see cref="UpdateCommentDto"/> to <see cref="DbAccess.Data.POCO.Comment"/>.
    /// </summary>
    public class UpdateCommentConverter : ITypeConverter<UpdateCommentDto, DbAccess.Data.POCO.Comment>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCommentConverter"/> class.
        /// </summary>
        /// <param name="commentRepository"></param>
        /// <param name="postRepository"></param>
        /// <param name="userRepository"></param>
        public UpdateCommentConverter(ICommentRepository commentRepository, IPostRepository postRepository,
            IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
            _userRepository = userRepository;
        }

        /// <inheritdoc />
        public DbAccess.Data.POCO.Comment Convert(UpdateCommentDto source, DbAccess.Data.POCO.Comment destination,
            ResolutionContext context)
        {
            destination.CommentParent = source.CommentParent != null ? _commentRepository.Get(source.CommentParent.Value) : null;
            destination.Content = source.Content;
            destination.PostParent = _postRepository.Get(source.PostParent);
            destination.Author = _userRepository.Get(source.Author);
            return destination;
        }
    }
}
