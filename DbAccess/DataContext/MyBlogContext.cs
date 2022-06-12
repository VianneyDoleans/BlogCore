using DBAccess.Data.POCO;
using DBAccess.Data.POCO.JoiningEntity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DBAccess.DataContext
{
    /// <summary>
    /// Context used for the API (Database, Entity Framework). It defined the tables and the relationship between them but also some default values.
    /// It enables (with also the attributes inside resource classes) to realize a Database code first generation (Entity Framework).
    /// </summary>
    public abstract class MyBlogContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MyBlogContext"/> class.
        /// </summary>
        /// <param name="options"></param>
        protected MyBlogContext(DbContextOptions options) : base(options) { }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PostTag> PostTags { get; set; }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("Identity");

            builder.Entity<PostTag>()
                .HasKey(pt => new { pt.PostId, pt.TagId });

            builder.Entity<UserRole>(entity =>
            {
                entity
                    .HasOne(x => x.Role)
                    .WithMany(x => x.UserRoles)
                    .HasForeignKey(x => x.RoleId);

                entity
                    .HasOne(x => x.User)
                    .WithMany(x => x.UserRoles)
                    .HasForeignKey(x => x.UserId);
            });

            builder.Entity<Like>().HasOne(s => s.User).WithMany(s => s.Likes).IsRequired().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Post>().HasOne(s => s.Category).WithMany(s => s.Posts).IsRequired().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Post>().HasOne(s => s.Author).WithMany(s => s.Posts).IsRequired().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Comment>().HasOne(s => s.Author).WithMany(s => s.Comments).IsRequired().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Comment>().HasOne(s => s.CommentParent).WithMany(s => s.ChildrenComments).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<User>(entity =>
            {
                entity.ToTable(name: "Users");
            });
            builder.Entity<Role>(entity =>
            {
                entity.ToTable(name: "Roles");
            });
            builder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            builder.Entity<IdentityUserClaim<int>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            builder.Entity<IdentityUserLogin<int>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            builder.Entity<IdentityRoleClaim<int>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            builder.Entity<IdentityUserToken<int>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
        }
    }
}
