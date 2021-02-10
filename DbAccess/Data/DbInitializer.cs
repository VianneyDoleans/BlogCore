using System.Collections.Generic;
using System.Linq;
using DbAccess.Data.POCO;
using DbAccess.Data.POCO.JoiningEntity;
using DbAccess.DataContext;

namespace DbAccess.Data
{
    public class DbInitializer
    {
        public static void Seed(MyBlogContext context)
        {
            if (!context.Roles.Any())
            {
                var userRole = new Role() {Name = "User"};
                var redactorRole = new Role() {Name = "Redactor"};
                var adminRole = new Role() {Name = "Admin"};
                context.Roles.Add(userRole);
                context.Roles.Add(adminRole);

                var sam = new User()
                {
                    EmailAddress = "Sam@email.com",
                    Username = "Sam",
                    Password = "1234"
                };
                var frodon = new User()
                {
                    EmailAddress = "fredon@email.com",
                    Username = "Frodon",
                    Password = "0000"
                };
                var jamy = new User()
                {
                    EmailAddress = "jamy@email.com",
                    Username = "Jamy",
                    Password = "JamyRedact",
                    UserDescription = "Hello, my name is Jamy, I love food"
                };
                var fred = new User()
                {
                    EmailAddress = "fred@email.com",
                    Username = "Fred",
                    Password = "FredRedact",
                };
                var admin = new User()
                {
                    EmailAddress = "admin@gmail.com",
                    Username = "AdminUser",
                    Password = "adminPassword",
                    UserDescription = "I'm admin, I manage this blog"
                };
                context.Users.Add(jamy);
                context.Users.Add(fred);
                context.Users.Add(sam);
                context.Users.Add(frodon);
                context.Users.Add(admin);

                context.UserRoles.Add(new UserRole() {Role = userRole, User = jamy});
                context.UserRoles.Add(new UserRole() {Role = userRole, User = fred});
                context.UserRoles.Add(new UserRole() {Role = userRole, User = frodon});
                context.UserRoles.Add(new UserRole() {Role = userRole, User = sam});
                context.UserRoles.Add(new UserRole() {Role = userRole, User = admin});
                context.UserRoles.Add(new UserRole() {Role = adminRole, User = admin});
                context.UserRoles.Add(new UserRole() {Role = redactorRole, User = fred});
                context.UserRoles.Add(new UserRole() {Role = redactorRole, User = jamy});

                var scienceTag = new Tag() {Name = "Science"};
                var journeyTag = new Tag {Name = "Journey"};
                var foodTag = new Tag {Name = "Food"};
                var natureTag = new Tag {Name = "Nature"};
                var japanTag = new Tag {Name = "Japan"};
                var singaporeTag = new Tag {Name = "Singapore"};
                var failureTag = new Tag {Name = "Failure"};
                context.Tags.Add(scienceTag);
                context.Tags.Add(journeyTag);
                context.Tags.Add(foodTag);
                context.Tags.Add(natureTag);
                context.Tags.Add(singaporeTag);
                context.Tags.Add(failureTag);
                context.Tags.Add(japanTag);

                var healthyFoodCategory = new Category() {Name = "HealthyFood"};
                var discoveryCategory = new Category() {Name = "Discovery"};
                var legendCategory = new Category() {Name = "Legend"};
                context.Categories.Add(healthyFoodCategory);
                context.Categories.Add(discoveryCategory);
                context.Categories.Add(legendCategory);

                var failFoodPost = new Post()
                {
                    Author = jamy,
                    Content = "This is a mess !",
                    Name = "I failed my pumpkin soup :'(",
                    Category = healthyFoodCategory,
                };
                failFoodPost.PostTags = new List<PostTag>()
                {
                    new PostTag() {Tag = failureTag, Post = failFoodPost},
                    new PostTag() {Tag = foodTag, Post = failFoodPost}
                };
                var volcanoes = new Post()
                {
                    Author = fred,
                    Content = "They are so cool !",
                    Name = "Volcanoes are cool",
                    Category = discoveryCategory
                };
                volcanoes.PostTags = new List<PostTag>()
                {
                    new PostTag() {Post = volcanoes, Tag = scienceTag},
                    new PostTag() {Post = volcanoes, Tag = natureTag}
                };
                var roadTrip = new Post()
                {
                    Author = fred,
                    Content = "Wellllllllcommmme ! =D End.",
                    Name = "Welcome to japan !",
                    Category = discoveryCategory
                };
                roadTrip.PostTags = new List<PostTag>()
                {
                    new PostTag() {Post = roadTrip, Tag = japanTag},
                    new PostTag() {Post = volcanoes, Tag = journeyTag}
                };
                var scaryVolcanoes = new Comment()
                {
                    Author = sam,
                    Content = "I don't like volcanoes, they are scary :'(",
                    PostParent = volcanoes
                };
                var weHaveToGo = new Comment()
                {
                    Author = frodon,
                    CommentParent = scaryVolcanoes,
                    Content = "Don't say that, We have to go to the ****** !",
                    PostParent = volcanoes
                };
                var samResponse = new Comment()
                {
                    Author = sam,
                    CommentParent = scaryVolcanoes,
                    Content = "No way !!",
                    PostParent = volcanoes
                };
                var fredAddDetail = new Comment()
                {
                    Author = fred,
                    Content = "Also, they are beautiful !",
                    PostParent = volcanoes
                };
                volcanoes.Comments = new List<Comment>()
                {
                    scaryVolcanoes,
                    weHaveToGo,
                    samResponse,
                    fredAddDetail
                };
                var frodonLike = new Like()
                {
                    Comment = fredAddDetail,
                    LikeableType = LikeableType.Comment,
                    User = frodon
                };
                var adminLike = new Like()
                {
                    Post = volcanoes,
                    LikeableType = LikeableType.Post,
                    User = admin
                };
                volcanoes.Likes = new List<Like>() {adminLike};
                fredAddDetail.Likes = new List<Like>() {frodonLike};

                context.Posts.Add(volcanoes);
                context.Posts.Add(failFoodPost);
                context.Posts.Add(roadTrip);

                context.SaveChanges();
            }
        }
    }
}
