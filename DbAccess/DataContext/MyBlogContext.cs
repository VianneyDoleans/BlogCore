using DbAccess.Data.POCO;
using DbAccess.Data.POCO.JoiningEntity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.DataContext
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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("Identity");

            modelBuilder.Entity<PostTag>()
                .HasKey(pt => new { pt.PostId, pt.TagId });

            modelBuilder.Entity<UserRole>(entity =>
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

            modelBuilder.Entity<Like>().HasOne(s => s.User).WithMany(s => s.Likes).IsRequired().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Post>().HasOne(s => s.Category).WithMany(s => s.Posts).IsRequired().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Post>().HasOne(s => s.Author).WithMany(s => s.Posts).IsRequired().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Comment>().HasOne(s => s.Author).WithMany(s => s.Comments).IsRequired().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Comment>().HasOne(s => s.CommentParent).WithMany(s => s.ChildrenComments).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable(name: "Users");
            });
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable(name: "Roles");
            });
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            modelBuilder.Entity<IdentityUserClaim<int>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            modelBuilder.Entity<IdentityUserLogin<int>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            modelBuilder.Entity<IdentityRoleClaim<int>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            modelBuilder.Entity<IdentityUserToken<int>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
        }
    }
}
