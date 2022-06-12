using System;
using DBAccess.Data.POCO;
using DBAccess.DataContext;
using DBAccess.Repositories.Like;
using DBAccess.Repositories.UnitOfWork;

namespace DBAccess.Tests.Builders
{
    public class LikeBuilder
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly BlogCoreContext _db;
        private User _user;
        private Comment _comment;
        private Post _post;

        public LikeBuilder(ILikeRepository likeRepository, IUnitOfWork unitOfWork, BlogCoreContext db)
        {
            _likeRepository = likeRepository;
            _unitOfWork = unitOfWork;
            _db = db;
        }

        public LikeBuilder WithUser(User user)
        {
            _user = user;
            return this;
        }

        public LikeBuilder WithComment(Comment comment)
        {
            _comment = comment;
            return this;
        }

        public LikeBuilder WithPost(Post post)
        {
            _post = post;
            return this;
        }

        public Like Build()
        {
            var user = _db.Users.Add(
                new User()
                {
                    Email = Guid.NewGuid().ToString("N") + "@test.com",
                    UserName = Guid.NewGuid().ToString()[..20],
                    Password = "testPassword"
                });
            var category = _db.Categories.Add(new Category() { Name = "GetLikesAsync" });
            var post = _db.Posts.Add(
                new Post()
                {
                    Author = user.Entity,
                    Name = Guid.NewGuid().ToString("N"),
                    Category = category.Entity,
                    Content = Guid.NewGuid().ToString("N")
                });
            var testLike = _likeRepository.Add(new Like()
            {
                User = user.Entity, 
                Post = post.Entity, 
                LikeableType = LikeableType.Post
            });
            if (_user != null)
                testLike.User = _user;
            if (_post != null && _comment != null)
                throw new Exception(nameof(testLike) + " can only have a post or a comment, not both at the same time.");
            if (_post != null)
            {
                testLike.LikeableType = LikeableType.Post;
                testLike.Post = _post;
            }
            else if (_comment != null)
            {
                testLike.LikeableType = LikeableType.Comment;
                testLike.Comment = _comment;
            }
            _unitOfWork.Save();
            return testLike;
        }
    }
}
