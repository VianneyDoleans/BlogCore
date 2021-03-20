using AutoMapper;
using DbAccess.Repositories.Comment;

namespace MyBlogAPI.DTO.Comment.Converters
{
    public class CommentIdConverter: ITypeConverter<int, DbAccess.Data.POCO.Comment>
    {
        private readonly ICommentRepository _repository;

        public CommentIdConverter(ICommentRepository repository)
        {
            _repository = repository;
        }

        public DbAccess.Data.POCO.Comment Convert(int source, DbAccess.Data.POCO.Comment destination, ResolutionContext context)
        {
            return _repository.Get(source);
        }
    }
}
