using System;
using DBAccess.Data;
using DBAccess.DataContext;
using DBAccess.Repositories.Comment;
using DBAccess.Repositories.UnitOfWork;

namespace DBAccess.Tests.Builders
{
    public class CommentBuilder
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly BlogCoreContext _db;
        private string _content;
        private User _author;

        public CommentBuilder(ICommentRepository commentRepository, IUnitOfWork unitOfWork, BlogCoreContext db)
        {
            _commentRepository = commentRepository;
            _unitOfWork = unitOfWork;
            _db = db;
        }

        public CommentBuilder WithContent(string content)
        {
            _content = content;
            return this;
        }

        public CommentBuilder WithAuthor(User author)
        {
            _author = author;
            return this;
        }

        public Comment Build()
        {
            var user = _db.Users.Add(
                new User()
                {
                    Email = Guid.NewGuid().ToString("N") + "@email.com",
                    Password = "1234",
                    UserName = Guid.NewGuid().ToString("N")[..20]
                });
            var category = _db.Categories.Add(new Category() { Name = Guid.NewGuid().ToString("N") });
            var post = _db.Posts.Add(
                new Post() {
                    Author = user.Entity,
                    Name = Guid.NewGuid().ToString("N"),
                    Category = category.Entity,
                    Content = Guid.NewGuid().ToString("N")
                });
            var testComment = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = Guid.NewGuid().ToString("N")
            };
            if (!string.IsNullOrEmpty(_content))
                testComment.Content = _content;
            if (_author != null)
                testComment.Author = _author;
            _commentRepository.Add(testComment);
            _unitOfWork.Save();
            return testComment;
        }
    }
}
