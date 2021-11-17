using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DbAccess.Data.POCO;
using DbAccess.Data.POCO.JoiningEntity;
using DbAccess.DataContext;

namespace DbAccess.Data
{
    /// <summary>
    /// Class used to initialize Database if the database is empty (test data).
    /// </summary>
    public class DbInitializer
    {
        private static void GenerateRoles(MyBlogContext context)
        {
            var userRole = new Role() { Name = "User" };
            var redactorRole = new Role() { Name = "Redactor" };
            var adminRole = new Role() { Name = "Admin" };
            context.Roles.AddRange(userRole, adminRole, redactorRole);
            context.SaveChanges();
        }

        private static void GenerateUsers(MyBlogContext context)
        {
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
            context.Users.AddRange(jamy, fred, sam, frodon, admin);
            context.SaveChanges();
        }

        private static void GenerateUserRoles(MyBlogContext context)
        {
            var admin = context.Roles.Single(x => x.Name == "Admin");
            var user = context.Roles.Single(x => x.Name == "User");
            var redactor = context.Roles.Single(x => x.Name == "Redactor");
            context.UserRoles.AddRange(
                new UserRole() {Role = user, User = context.Users.Single(x => x.Username == "Jamy")},
                new UserRole() {Role = user, User = context.Users.Single(x => x.Username == "Fred")},
                new UserRole() {Role = user, User = context.Users.Single(x => x.Username == "Frodon")},
                new UserRole() {Role = user, User = context.Users.Single(x => x.Username == "Sam")},
                new UserRole() {Role = user, User = context.Users.Single(x => x.Username == "AdminUser")},
                new UserRole() {Role = admin, User = context.Users.Single(x => x.Username == "AdminUser")},
                new UserRole() {Role = redactor, User = context.Users.Single(x => x.Username == "Fred")},
                new UserRole() {Role = redactor, User = context.Users.Single(x => x.Username == "Jamy")});
            context.SaveChanges();
        }

        private static void GenerateTags(MyBlogContext context)
        {
            var scienceTag = new Tag() { Name = "Science" };
            var journeyTag = new Tag { Name = "Journey" };
            var foodTag = new Tag { Name = "Food" };
            var natureTag = new Tag { Name = "Nature" };
            var japanTag = new Tag { Name = "Japan" };
            var singaporeTag = new Tag { Name = "Singapore" };
            var failureTag = new Tag { Name = "Failure" };
            context.Tags.AddRange(scienceTag, journeyTag, foodTag, natureTag, singaporeTag, failureTag, japanTag);
            context.SaveChanges();
        }

        private static void GenerateCategories(MyBlogContext context)
        {
            var healthyFoodCategory = new Category() { Name = "HealthyFood" };
            var discoveryCategory = new Category() { Name = "Discovery" };
            var legendCategory = new Category() { Name = "Legend" };
            context.Categories.Add(healthyFoodCategory);
            context.Categories.Add(discoveryCategory);
            context.Categories.Add(legendCategory);
            context.Categories.AddRange(healthyFoodCategory, discoveryCategory, legendCategory);
            context.SaveChanges();
        }

        private static void GeneratePostTags(MyBlogContext context)
        {
            var failFoodPost = context.Posts.Single(x => x.Name == "I failed my pumpkin soup :'(");
            var volcanoes = context.Posts.Single(x => x.Name == "Volcanoes are cool");
            var roadTrip = context.Posts.Single(x => x.Name == "Welcome to japan !");

            var failureFailFoodPost = new PostTag() {Tag = context.Tags.Single(x => x.Name == "Failure"), Post = failFoodPost};
            var foodFailFoodPost = new PostTag() {Tag = context.Tags.Single(x => x.Name == "Food"), Post = failFoodPost};
            var volcanoesScience = new PostTag() {Post = volcanoes, Tag = context.Tags.Single(x => x.Name == "Science")};
            var volcanoesNature = new PostTag() {Post = volcanoes, Tag = context.Tags.Single(x => x.Name == "Nature")};
            var japanRoadTrip = new PostTag() {Post = roadTrip, Tag = context.Tags.Single(x => x.Name == "Japan")};
            var volcanoesJourney = new PostTag() {Post = volcanoes, Tag = context.Tags.Single(x => x.Name == "Journey")};

            context.PostTags.AddRange(japanRoadTrip, volcanoesJourney, volcanoesNature, volcanoesScience, foodFailFoodPost, failureFailFoodPost);
            context.SaveChanges();
        }

        private static void GeneratePosts(MyBlogContext context)
        {
            
            var failFoodPost = new Post()
            {
                Author = context.Users.Single(x => x.Username == "Jamy"), 
                Content = "This is a mess !",
                Name = "I failed my pumpkin soup :'(",
                Category = context.Categories.Single(x => x.Name == "HealthyFood")
            };
            
            var volcanoes = new Post()
            {
                Author = context.Users.Single(x => x.Username == "Fred"), 
                Content = "They are so cool !",
                Name = "Volcanoes are cool",
                Category = context.Categories.Single(x => x.Name == "Discovery"),
            };
            
            var roadTrip = new Post()
            {
                Author = context.Users.Single(x => x.Username == "Fred"),
                Content = "Wellllllllcommmme ! =D End.",
                Name = "Welcome to japan !",
                Category = context.Categories.Single(x => x.Name == "HealthyFood"),
            };
            context.Posts.AddRange(volcanoes, failFoodPost, roadTrip);
            context.SaveChanges();
        }

        private static void GenerateLikes(MyBlogContext context)
        {
            var fredAddDetail = context.Comments.Single(x => x.Author.Username == "Fred" && x.Content == "Also, they are beautiful !");
            var volcanoes = context.Posts.Single(x => x.Name == "Volcanoes are cool");
            var frodonLike = new Like()
            {
                Comment = fredAddDetail,
                LikeableType = LikeableType.Comment,
                User = context.Users.Single(x => x.Username == "Frodon"),
            };
            var adminLike = new Like()
            {
                Post = volcanoes,
                LikeableType = LikeableType.Post,
                User = context.Users.Single(x => x.Username == "AdminUser"),
            };
            volcanoes.Likes = new List<Like>() { adminLike };
            fredAddDetail.Likes = new List<Like>() { frodonLike };
            context.SaveChanges();
        }

        private static void GenerateComments(MyBlogContext context)
        {
            var volcanoes = context.Posts.Single(x => x.Name == "Volcanoes are cool");
            var scaryVolcanoes = new Comment()
            {
                Author = context.Users.Single(x => x.Username == "Sam"),
                Content = "I don't like volcanoes, they are scary :'(",
                PostParent = volcanoes
            };
            var weHaveToGo = new Comment()
            {
                Author = context.Users.Single(x => x.Username == "Frodon"),
                CommentParent = scaryVolcanoes,
                Content = "Don't say that, We have to go to the ****** !",
                PostParent = volcanoes
            };
            var samResponse = new Comment()
            {
                Author = context.Users.Single(x => x.Username == "Sam"),
                CommentParent = scaryVolcanoes,
                Content = "No way !!",
                PostParent = volcanoes
            };
            var fredAddDetail = new Comment()
            {
                Author = context.Users.Single(x => x.Username == "Fred"),
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
            context.SaveChanges();
        }

        /// <summary>
        /// Fill the database with <see cref="MyBlogContext"/> (Entity Framework).
        /// The methods will fill the database only if no <see cref="DbAccess.Data.POCO.Role"/> exists
        /// (No existing roles means that the database is still empty / not used).
        /// </summary>
        /// <param name="context"></param>
        public static void Seed(MyBlogContext context)
        {
            Console.WriteLine("DEBUG HERE");
            if (context?.Roles == null)
                Console.WriteLine("role is null.");
            Console.WriteLine("role number : " + context.Roles.Count());
            if (!context.Roles.Any())
            {
                GenerateRoles(context);
                GenerateUsers(context);
                GenerateUserRoles(context);
                GenerateTags(context);
                GenerateCategories(context);
                GeneratePosts(context);
                GeneratePostTags(context);
                GenerateComments(context);
                GenerateLikes(context);
            }
        }
    }
}
