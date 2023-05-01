using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBAccess.Builders;
using DBAccess.Data;
using DBAccess.Data.JoiningEntity;
using DBAccess.Data.Permission;
using DBAccess.DataContext;
using Microsoft.AspNetCore.Identity;

namespace DBAccess
{
    /// <summary>
    /// Class used to initialize Database if the database is empty (test data).
    /// </summary>
    public static class DbInitializer
    {
        private const string UserRole = "User";
        private const string RedactorRole = "Redactor";
        private const string AdminRole = "Admin";

        private static async Task GenerateDefaultRoles(RoleManager<Role> roleManager, BlogCoreContext context)
        {
            var userRole = await new RoleBuilder(roleManager)
                .WithName(UserRole)
                .WithCanReadAllOnAllResourcesExceptAccount()
                .WithCanCreateOwn(PermissionTarget.Comment)
                .WithCanUpdateOwn(PermissionTarget.Comment)
                .WithCanCreateOwn(PermissionTarget.Like)
                .WithCanUpdateOwn(PermissionTarget.Like)
                .WithCanDeleteOwn(PermissionTarget.Comment)
                .WithCanDeleteOwn(PermissionTarget.Like)
                .WithCanReadOwn(PermissionTarget.Account)
                .WithCanDeleteOwn(PermissionTarget.Account)
                .WithCanUpdateOwn(PermissionTarget.Account)
                .Build();

            await new RoleBuilder(roleManager)
                .WithName(RedactorRole)
                .WithCanCreateAll(PermissionTarget.Category)
                .WithCanCreateAll(PermissionTarget.Tag)
                .WithCanCreateOwn(PermissionTarget.Post)
                .WithCanUpdateOwn(PermissionTarget.Post)
                .WithCanDeleteOwn(PermissionTarget.Post)
                .Build();

            await new RoleBuilder(roleManager)
                .WithName(AdminRole)
                .WithCanReadAllOnAllResourcesExceptAccount()
                .WithCanCreateAllOnAllResources()
                .WithCanUpdateAllOnAllResources()
                .WithCanDeleteAllOnAllResources()
                .WithCanReadAll(PermissionTarget.Account)
                .Build();

            await context.DefaultRoles.AddAsync(new DefaultRoles() { Role = userRole });
            await context.SaveChangesAsync();
        }

        private static async Task GenerateDefaultUsers(UserManager<User> userManager)
        {
            var users = new List<(User, string)>()
            {
                (new UserBuilder().WithEmail("Sam@email.com").WithUsername("Sam").Build(), "0a1234A@"),
                (new UserBuilder().WithEmail("fredon@email.com").WithUsername("Frodon").Build(), "0a0000A@"),
                (new UserBuilder().WithEmail("jamy@email.com").WithUsername("Jamy")
                    .WithDescription("Hello, my name is Jamy, I love food").Build(), "0JamyRedactA@"),
                (new UserBuilder().WithEmail("fred@email.com").WithUsername("Fred").Build(), "0FredRedactA@"),
                (new UserBuilder().WithEmail("admin@emailblogcore.com").WithUsername("AdminUser")
                    .WithDescription("I'm admin, I manage this blog").Build(), "0adminPasswordA@")
            };
            foreach (var user in users)
            {
                await userManager.CreateAsync(user.Item1, user.Item2);
                var emailConfirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user.Item1);
                await userManager.ConfirmEmailAsync(user.Item1, emailConfirmationToken);
            }
        }

        private static void AssignRolesToDefaultUsers(BlogCoreContext context)
        {
            context.UserRoles.AddRange(
                new UserRoleBuilder(context).WithUser("Jamy").WithRole(UserRole).Build(),
                new UserRoleBuilder(context).WithUser("Fred").WithRole(UserRole).Build(),
                new UserRoleBuilder(context).WithUser("Frodon").WithRole(UserRole).Build(),
                new UserRoleBuilder(context).WithUser("Sam").WithRole(UserRole).Build(),
                new UserRoleBuilder(context).WithUser("AdminUser").WithRole(UserRole).Build(),
                
                new UserRoleBuilder(context).WithUser("Fred").WithRole(RedactorRole).Build(),
                new UserRoleBuilder(context).WithUser("Jamy").WithRole(RedactorRole).Build(),

                new UserRoleBuilder(context).WithUser("AdminUser").WithRole(AdminRole).Build()
            );
            context.SaveChanges();
        }

        private static void GenerateDefaultTags(BlogCoreContext context)
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

        private static void GenerateDefaultCategories(BlogCoreContext context)
        {
            var healthyFoodCategory = new Category() { Name = "HealthyFood" };
            var discoveryCategory = new Category() { Name = "Discovery" };
            var legendCategory = new Category() { Name = "Legend" };
            context.Categories.AddRange(healthyFoodCategory, discoveryCategory, legendCategory);
            context.SaveChanges();
        }

        private static void GenerateDefaultPostTags(BlogCoreContext context)
        {
            var failFoodPost = context.Posts.Single(x => x.Name == "I failed my pumpkin soup :'(");
            var volcanoes = context.Posts.Single(x => x.Name == "Volcanoes are cool");
            var roadTrip = context.Posts.Single(x => x.Name == "Welcome to japan !");

            var failureFailFoodPost = new PostTag()
                { Tag = context.Tags.Single(x => x.Name == "Failure"), Post = failFoodPost };
            var foodFailFoodPost = new PostTag()
                { Tag = context.Tags.Single(x => x.Name == "Food"), Post = failFoodPost };
            var volcanoesScience = new PostTag()
                { Post = volcanoes, Tag = context.Tags.Single(x => x.Name == "Science") };
            var volcanoesNature = new PostTag()
                { Post = volcanoes, Tag = context.Tags.Single(x => x.Name == "Nature") };
            var japanRoadTrip = new PostTag() { Post = roadTrip, Tag = context.Tags.Single(x => x.Name == "Japan") };
            var volcanoesJourney = new PostTag()
                { Post = volcanoes, Tag = context.Tags.Single(x => x.Name == "Journey") };

            context.PostTags.AddRange(japanRoadTrip, volcanoesJourney, volcanoesNature, volcanoesScience,
                foodFailFoodPost, failureFailFoodPost);
            context.SaveChanges();
        }

        private static void GenerateDefaultPosts(BlogCoreContext context)
        {

            var failFoodPost = new Post()
            {
                Author = context.Users.Single(x => x.UserName == "Jamy"),
                Content = "This is a mess !",
                Name = "I failed my pumpkin soup :'(",
                Category = context.Categories.Single(x => x.Name == "HealthyFood")
            };

            var volcanoes = new Post()
            {
                Author = context.Users.Single(x => x.UserName == "Fred"),
                Content = "They are so cool !",
                Name = "Volcanoes are cool",
                Category = context.Categories.Single(x => x.Name == "Discovery"),
            };

            var roadTrip = new Post()
            {
                Author = context.Users.Single(x => x.UserName == "Fred"),
                Content = "Wellllllllcommmme ! =D End.",
                Name = "Welcome to japan !",
                Category = context.Categories.Single(x => x.Name == "HealthyFood"),
            };
            context.Posts.AddRange(volcanoes, failFoodPost, roadTrip);
            context.SaveChanges();
        }

        private static void GenerateDefaultLikes(BlogCoreContext context)
        {
            var fredAddDetail = context.Comments.Single(x =>
                x.Author.UserName == "Fred" && x.Content == "Also, they are beautiful !");
            var volcanoes = context.Posts.Single(x => x.Name == "Volcanoes are cool");
            var frodonLike = new Like()
            {
                Comment = fredAddDetail,
                LikeableType = LikeableType.Comment,
                User = context.Users.Single(x => x.UserName == "Frodon"),
            };
            var adminLike = new Like()
            {
                Post = volcanoes,
                LikeableType = LikeableType.Post,
                User = context.Users.Single(x => x.UserName == "AdminUser"),
            };
            volcanoes.Likes = new List<Like>() { adminLike };
            fredAddDetail.Likes = new List<Like>() { frodonLike };
            context.SaveChanges();
        }

        private static void GenerateDefaultComments(BlogCoreContext context)
        {
            var volcanoes = context.Posts.Single(x => x.Name == "Volcanoes are cool");

            var scaryVolcanoes = 
                new Comment()
            {
                Author = context.Users.Single(x => x.UserName == "Sam"),
                Content = "I don't like volcanoes, they are scary :'(",
                PostParent = volcanoes
            };
            var weHaveToGo = new Comment()
            {
                Author = context.Users.Single(x => x.UserName == "Frodon"),
                CommentParent = scaryVolcanoes,
                Content = "Don't say that, We have to go to the ****** !",
                PostParent = volcanoes
            };
            var samResponse = new Comment()
            {
                Author = context.Users.Single(x => x.UserName == "Sam"),
                CommentParent = scaryVolcanoes,
                Content = "No way !!",
                PostParent = volcanoes
            };
            var fredAddDetail = new Comment()
            {
                Author = context.Users.Single(x => x.UserName == "Fred"),
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
        /// Fill the database with <see cref="BlogCoreContext"/> (Entity Framework).
        /// The methods will fill the database only if no <see cref="Role"/> exists
        /// (No existing roles means that the database is still empty / not used).
        /// </summary>
        /// <param name="context"></param>
        /// <param name="roleManager"></param>
        /// <param name="userManager"></param>
        public static async Task SeedWithDefaultValues(BlogCoreContext context, RoleManager<Role> roleManager,
            UserManager<User> userManager)
        {
            if (!context.Roles.Any())
            {
                await GenerateDefaultRoles(roleManager, context);
                await GenerateDefaultUsers(userManager);
                AssignRolesToDefaultUsers(context);
                GenerateDefaultTags(context);
                GenerateDefaultCategories(context);
                GenerateDefaultPosts(context);
                GenerateDefaultPostTags(context);
                GenerateDefaultComments(context);
                GenerateDefaultLikes(context);
            }
        }
    }
}
