using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
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
        private static async Task GenerateRoles(RoleManager<Role> roleManager)
        {
            var userRole = new Role() { Name = "User" };
            await roleManager.CreateAsync(userRole);
            var userPermissions = new List<Permission>()
            {
                // Read
                new()
                {
                    PermissionAction = PermissionAction.CanRead, PermissionTarget = PermissionTarget.Category,
                    PermissionRange = PermissionRange.All
                },
                new()
                {
                    PermissionAction = PermissionAction.CanRead, PermissionTarget = PermissionTarget.Comment,
                    PermissionRange = PermissionRange.All
                },
                new()
                {
                    PermissionAction = PermissionAction.CanRead, PermissionTarget = PermissionTarget.Like,
                    PermissionRange = PermissionRange.All
                },
                new()
                {
                    PermissionAction = PermissionAction.CanRead, PermissionTarget = PermissionTarget.Post,
                    PermissionRange = PermissionRange.All
                },
                new()
                {
                    PermissionAction = PermissionAction.CanRead, PermissionTarget = PermissionTarget.Role,
                    PermissionRange = PermissionRange.All
                },
                new()
                {
                    PermissionAction = PermissionAction.CanRead, PermissionTarget = PermissionTarget.Tag,
                    PermissionRange = PermissionRange.All
                },
                new()
                {
                    PermissionAction = PermissionAction.CanRead, PermissionTarget = PermissionTarget.User,
                    PermissionRange = PermissionRange.All
                },

                // Create
                new()
                {
                    PermissionAction = PermissionAction.CanCreate, PermissionTarget = PermissionTarget.Comment,
                    PermissionRange = PermissionRange.Own
                },
                new()
                {
                    PermissionAction = PermissionAction.CanCreate, PermissionTarget = PermissionTarget.Like,
                    PermissionRange = PermissionRange.Own
                },

                // Update
                new()
                {
                    PermissionAction = PermissionAction.CanUpdate, PermissionTarget = PermissionTarget.Comment,
                    PermissionRange = PermissionRange.Own
                },
                new()
                {
                    PermissionAction = PermissionAction.CanUpdate, PermissionTarget = PermissionTarget.Like,
                    PermissionRange = PermissionRange.Own
                },
                new()
                {
                    PermissionAction = PermissionAction.CanUpdate, PermissionTarget = PermissionTarget.User,
                    PermissionRange = PermissionRange.Own
                },

                // Delete
                new()
                {
                    PermissionAction = PermissionAction.CanDelete, PermissionTarget = PermissionTarget.Comment,
                    PermissionRange = PermissionRange.Own
                },
                new()
                {
                    PermissionAction = PermissionAction.CanDelete, PermissionTarget = PermissionTarget.Like,
                    PermissionRange = PermissionRange.Own
                }
            };
            foreach (var permission in userPermissions)
            { 
                await roleManager.AddClaimAsync(userRole, 
                    new Claim("Permission", JsonSerializer.Serialize(permission)));
            }

            var redactorRole = new Role() { Name = "Redactor" };
            await roleManager.CreateAsync(redactorRole);
            var redactorPermissions = new List<Permission>()
            {
                // Create
                new()
                {
                    PermissionAction = PermissionAction.CanCreate, PermissionTarget = PermissionTarget.Category,
                    PermissionRange = PermissionRange.All
                },
                new()
                {
                    PermissionAction = PermissionAction.CanCreate, PermissionTarget = PermissionTarget.Tag,
                    PermissionRange = PermissionRange.All
                },
                new()
                {
                    PermissionAction = PermissionAction.CanCreate, PermissionTarget = PermissionTarget.Post,
                    PermissionRange = PermissionRange.Own
                },

                // Update
                new()
                {
                    PermissionAction = PermissionAction.CanUpdate, PermissionTarget = PermissionTarget.Post,
                    PermissionRange = PermissionRange.Own
                },

                // Delete
                new()
                {
                    PermissionAction = PermissionAction.CanDelete, PermissionTarget = PermissionTarget.Post,
                    PermissionRange = PermissionRange.Own
                }
            };
            foreach (var permission in redactorPermissions)
            {
                await roleManager.AddClaimAsync(redactorRole,
                    new Claim("Permission", JsonSerializer.Serialize(permission)));
            }

            var adminRole = new Role() { Name = "Admin" };
            await roleManager.CreateAsync(adminRole);
            var adminPermissions = new List<Permission>()
            {
                // Read
                new()
                {
                    PermissionAction = PermissionAction.CanRead, PermissionTarget = PermissionTarget.Category,
                    PermissionRange = PermissionRange.All
                },
                new()
                {
                    PermissionAction = PermissionAction.CanRead, PermissionTarget = PermissionTarget.Comment,
                    PermissionRange = PermissionRange.All
                },
                new()
                {
                    PermissionAction = PermissionAction.CanRead, PermissionTarget = PermissionTarget.Like,
                    PermissionRange = PermissionRange.All
                },
                new()
                {
                    PermissionAction = PermissionAction.CanRead, PermissionTarget = PermissionTarget.Post,
                    PermissionRange = PermissionRange.All
                },
                new()
                {
                    PermissionAction = PermissionAction.CanRead, PermissionTarget = PermissionTarget.Role,
                    PermissionRange = PermissionRange.All
                },
                new()
                {
                    PermissionAction = PermissionAction.CanRead, PermissionTarget = PermissionTarget.Tag,
                    PermissionRange = PermissionRange.All
                },
                new()
                {
                    PermissionAction = PermissionAction.CanRead, PermissionTarget = PermissionTarget.User,
                    PermissionRange = PermissionRange.All
                },

                // Create
                new()
                {
                    PermissionAction = PermissionAction.CanCreate, PermissionTarget = PermissionTarget.Category,
                    PermissionRange = PermissionRange.All
                },
                new()
                {
                    PermissionAction = PermissionAction.CanCreate, PermissionTarget = PermissionTarget.Comment,
                    PermissionRange = PermissionRange.All
                },
                new()
                {
                    PermissionAction = PermissionAction.CanCreate, PermissionTarget = PermissionTarget.Like,
                    PermissionRange = PermissionRange.All
                },
                new()
                {
                    PermissionAction = PermissionAction.CanCreate, PermissionTarget = PermissionTarget.Post,
                    PermissionRange = PermissionRange.All
                },
                new()
                {
                    PermissionAction = PermissionAction.CanCreate, PermissionTarget = PermissionTarget.Role,
                    PermissionRange = PermissionRange.All
                },
                new()
                {
                    PermissionAction = PermissionAction.CanCreate, PermissionTarget = PermissionTarget.Tag,
                    PermissionRange = PermissionRange.All
                },
                new()
                {
                    PermissionAction = PermissionAction.CanCreate, PermissionTarget = PermissionTarget.User,
                    PermissionRange = PermissionRange.All
                },

                // Update
                new()
                {
                    PermissionAction = PermissionAction.CanUpdate, PermissionTarget = PermissionTarget.Category,
                    PermissionRange = PermissionRange.All
                },
                new()
                {
                    PermissionAction = PermissionAction.CanUpdate, PermissionTarget = PermissionTarget.Comment,
                    PermissionRange = PermissionRange.All
                },
                new()
                {
                    PermissionAction = PermissionAction.CanUpdate, PermissionTarget = PermissionTarget.Like,
                    PermissionRange = PermissionRange.All
                },
                new()
                {
                    PermissionAction = PermissionAction.CanUpdate, PermissionTarget = PermissionTarget.Post,
                    PermissionRange = PermissionRange.All
                },
                new()
                {
                    PermissionAction = PermissionAction.CanUpdate, PermissionTarget = PermissionTarget.Role,
                    PermissionRange = PermissionRange.All
                },
                new()
                {
                    PermissionAction = PermissionAction.CanUpdate, PermissionTarget = PermissionTarget.Tag,
                    PermissionRange = PermissionRange.All
                },
                new()
                {
                    PermissionAction = PermissionAction.CanUpdate, PermissionTarget = PermissionTarget.User,
                    PermissionRange = PermissionRange.All
                },

                // Delete
                new()
                {
                    PermissionAction = PermissionAction.CanDelete, PermissionTarget = PermissionTarget.Category,
                    PermissionRange = PermissionRange.All
                },
                new()
                {
                    PermissionAction = PermissionAction.CanDelete, PermissionTarget = PermissionTarget.Comment,
                    PermissionRange = PermissionRange.All
                },
                new()
                {
                    PermissionAction = PermissionAction.CanDelete, PermissionTarget = PermissionTarget.Like,
                    PermissionRange = PermissionRange.All
                },
                new()
                {
                    PermissionAction = PermissionAction.CanDelete, PermissionTarget = PermissionTarget.Post,
                    PermissionRange = PermissionRange.All
                },
                new()
                {
                    PermissionAction = PermissionAction.CanDelete, PermissionTarget = PermissionTarget.Role,
                    PermissionRange = PermissionRange.All
                },
                new()
                {
                    PermissionAction = PermissionAction.CanDelete, PermissionTarget = PermissionTarget.Tag,
                    PermissionRange = PermissionRange.All
                },
                new()
                {
                    PermissionAction = PermissionAction.CanDelete, PermissionTarget = PermissionTarget.User,
                    PermissionRange = PermissionRange.All
                },
            };
            foreach (var permission in adminPermissions)
            {
                await roleManager.AddClaimAsync(adminRole,
                    new Claim("Permission", JsonSerializer.Serialize(permission)));
            }
        }

        private static async Task GenerateUsers(UserManager<User> userManager)
        {
            var users = new List<(User, string)>()
            {
                (new User
                {
                    Email = "Sam@email.com",
                    UserName = "Sam",
                }, "0a1234A@"),
                (new User()
                {
                    Email = "fredon@email.com",
                    UserName = "Frodon",
                }, "0a0000A@"),
                (new User()
                {
                    Email = "jamy@email.com",
                    UserName = "Jamy",
                    UserDescription = "Hello, my name is Jamy, I love food"
                }, "0JamyRedactA@"),
                (new User()
                {
                    Email = "fred@email.com",
                    UserName = "Fred",
                }, "0FredRedactA@"),
                (new User()
                {
                    Email = "admin@gmail.com",
                    UserName = "AdminUser",
                    UserDescription = "I'm admin, I manage this blog"
                }, "0adminPasswordA@")
            };
            foreach (var user in users)
            {
                await userManager.CreateAsync(user.Item1, user.Item2);
            }
        }

        private static void GenerateUserRoles(BlogCoreContext context)
        {
            var admin = context.Roles.Single(x => x.Name == "Admin");
            var user = context.Roles.Single(x => x.Name == "User");
            var redactor = context.Roles.Single(x => x.Name == "Redactor");
            context.UserRoles.AddRange(
                new UserRole() { Role = user, User = context.Users.Single(x => x.UserName == "Jamy") },
                new UserRole() { Role = user, User = context.Users.Single(x => x.UserName == "Fred") },
                new UserRole() { Role = user, User = context.Users.Single(x => x.UserName == "Frodon") },
                new UserRole() { Role = user, User = context.Users.Single(x => x.UserName == "Sam") },
                new UserRole() { Role = user, User = context.Users.Single(x => x.UserName == "AdminUser") },
                new UserRole() { Role = admin, User = context.Users.Single(x => x.UserName == "AdminUser") },
                new UserRole() { Role = redactor, User = context.Users.Single(x => x.UserName == "Fred") },
                new UserRole() { Role = redactor, User = context.Users.Single(x => x.UserName == "Jamy") });
            context.SaveChanges();
        }

        private static void GenerateTags(BlogCoreContext context)
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

        private static void GenerateCategories(BlogCoreContext context)
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

        private static void GeneratePostTags(BlogCoreContext context)
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

        private static void GeneratePosts(BlogCoreContext context)
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

        private static void GenerateLikes(BlogCoreContext context)
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

        private static void GenerateComments(BlogCoreContext context)
        {
            var volcanoes = context.Posts.Single(x => x.Name == "Volcanoes are cool");
            var scaryVolcanoes = new Comment()
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
        public static async Task Seed(BlogCoreContext context, RoleManager<Role> roleManager,
            UserManager<User> userManager)
        {
            if (!context.Roles.Any())
            {
                await GenerateRoles(roleManager);
                await GenerateUsers(userManager);
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
