using DbAccess.Data.JoiningEntity;
using DbAccess.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.DataContext
{
    public class MyBlogContext : DbContext
    {
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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostTag>()
                .HasKey(pt => new { pt.PostId, pt.TagId });

            /*modelBuilder.Entity<ArticleTag>()
                .HasOne(s => s.Article)
                .WithMany(c => c.ArticleTags)
                .HasForeignKey(sc => sc.ArticleId);
            modelBuilder.Entity<ArticleTag>()
                .HasOne(s => s.Tag)
                .WithMany(c => c.ArticleTags)
                .HasForeignKey(sc => sc.TagId);*/

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });
            /*modelBuilder.Entity<UserRole>()
                .HasOne(s => s.User)
                .WithMany(c => c.UserRoles)
                .HasForeignKey(sc => sc.UserId);
            modelBuilder.Entity<UserRole>()
                .HasOne(s => s.Role)
                .WithMany(c => c.UserRoles)
                .HasForeignKey(sc => sc.RoleId);
            base.OnModelCreating(modelBuilder);*/

            modelBuilder.Entity<User>().Property(b => b.RegisteredAt).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<User>().Property(b => b.LastLogin).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Comment>().Property(b => b.PublishedAt).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Post>().Property(b => b.PublishedAt).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Like>().Property(b => b.PublishedAt).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Comment>().Property(b => b.PublishedAt).HasDefaultValueSql("GETUTCDATE()");

            //modelBuilder.Entity<Like>().HasOne(s => s.Comment).WithMany(s => s.Likes).HasForeignKey(s => s.Id).IsRequired().OnDelete(DeleteBehavior.NoAction);
            //modelBuilder.Entity<Like>().HasOne(s => s.Post).WithMany(s => s.Likes).HasForeignKey(s => s.Id).IsRequired().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Like>().HasOne(s => s.User).WithMany(s => s.Likes).IsRequired().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Post>().HasOne(s => s.Category).WithMany(s => s.Posts).IsRequired().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Post>().HasOne(s => s.Author).WithMany(s => s.Posts).IsRequired().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Comment>().HasOne(s => s.Author).WithMany(s => s.Comments).IsRequired().OnDelete(DeleteBehavior.NoAction);
        }
    }
}
