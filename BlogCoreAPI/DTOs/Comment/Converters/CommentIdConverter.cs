using AutoMapper;
using DBAccess.Data;
using DBAccess.Repositories.Comment;

namespace BlogCoreAPI.DTOs.Comment.Converters
{
    /// <summary>
    /// AutoMapper converter used to enable the conversion of <see cref="Comment"/> to its resource Id.
    /// </summary>
    public class CommentIdConverter: ITypeConverter<int, DBAccess.Data.Comment>
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
        public DBAccess.Data.Comment Convert(int source, DBAccess.Data.Comment destination, ResolutionContext context)
        {
            return _repository.Get(source);
        }
    }
}
