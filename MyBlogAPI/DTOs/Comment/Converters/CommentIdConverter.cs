using AutoMapper;
using DbAccess.Repositories.Comment;

namespace MyBlogAPI.DTOs.Comment.Converters
{
    /// <summary>
    /// AutoMapper converter used to enable the conversion of <see cref="DbAccess.Data.POCO.Comment"/> to its resource Id.
    /// </summary>
    public class CommentIdConverter: ITypeConverter<int, DbAccess.Data.POCO.Comment>
    {
        private readonly ICommentRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentIdConverter"/> class.
        /// </summary>
        /// <param name="repository"></param>
        public CommentIdConverter(ICommentRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc />
        public DbAccess.Data.POCO.Comment Convert(int source, DbAccess.Data.POCO.Comment destination, ResolutionContext context)
        {
            return _repository.Get(source);
        }
    }
}
