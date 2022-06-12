using System;
using DBAccess.Data.POCO;
using DBAccess.DataContext;
using DBAccess.Repositories.Post;
using DBAccess.Repositories.UnitOfWork;

namespace DBAccess.Tests.Builders
{
    public class PostBuilder
    {
        private readonly IPostRepository _postRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly BlogCoreContext _db;
        private string _name;
        private User _user;
        private Category _category;
        private string _content;

        public PostBuilder(IPostRepository postRepository, IUnitOfWork unitOfWork, BlogCoreContext db)
        {
            _postRepository = postRepository;
            _unitOfWork = unitOfWork;
            _db = db;
        }

        public PostBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public PostBuilder WithAuthor(User user)
        {
            _user = user;
            return this;
        }

        public PostBuilder WithCategory(Category category)
        {
            _category = category;
            return this;
        }

        public PostBuilder WithContent(string content)
        {
            _content = content;
            return this;
        }

        public Post Build()
        {
            var user = _db.Users.Add(
                new User()
                {
                    Email = Guid.NewGuid().ToString("N") + "@email.com",
                    Password = "1234", 
                    UserName = Guid.NewGuid().ToString("N")[..20]
                });
            var category = _db.Categories.Add(new Category() { Name = Guid.NewGuid().ToString("N") });
            var testPost = new Post() { 
                Author = user.Entity,
                Name = Guid.NewGuid().ToString("N"),
                Category = category.Entity,
                Content = Guid.NewGuid().ToString("N")
            };

            if (!string.IsNullOrEmpty(_name))
                testPost.Name = _name;
            if (_category != null)
                testPost.Category = _category;
            if (_user != null)
                testPost.Author = _user;
            if (_content != null)
                testPost.Content = _content;
            _postRepository.Add(testPost);
            _unitOfWork.Save();
            return testPost;
        }
    }
}
