using DbAccess.Data.POCO;
using DbAccess.Data.POCO.JoiningEntity;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.DataContext
{
    /// <summary>
    /// Context used for the API (Database, Entity Framework). It defined the tables and the relationship between them but also some default values.
    /// It enables (with also the attributes inside resource classes) to realize a Database code first generation (Entity Framework).
    /// </summary>
    public class MyBlogContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MyBlogContext"/> class.
        /// </summary>
        /// <param name="options"></param>
        public MyBlogContext(DbContextOptions<MyBlogContext> options) : base(options) { }

        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostTag>()
                .HasKey(pt => new { pt.PostId, pt.TagId });
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<User>().Property(b => b.RegisteredAt).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<User>().Property(b => b.LastLogin).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Comment>().Property(b => b.PublishedAt).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Post>().Property(b => b.PublishedAt).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Like>().Property(b => b.PublishedAt).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Comment>().Property(b => b.PublishedAt).HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Like>().HasOne(s => s.User).WithMany(s => s.Likes).IsRequired().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Post>().HasOne(s => s.Category).WithMany(s => s.Posts).IsRequired().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Post>().HasOne(s => s.Author).WithMany(s => s.Posts).IsRequired().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Comment>().HasOne(s => s.Author).WithMany(s => s.Comments).IsRequired().OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<Comment>().HasOne(s => s.CommentParent).WithMany(s => s.ChildrenComments).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
