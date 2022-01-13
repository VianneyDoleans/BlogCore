using DbAccess.Data.POCO;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.DataContext
{
    /// <summary>
    /// Context used for Microsoft SQL Server database (compatibility)
    /// </summary>
    public class MsSqlDbContext : MyBlogContext
    {
        /// <inheritdoc />
        public MsSqlDbContext(DbContextOptions options) : base(options) { }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().Property(b => b.RegisteredAt).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<User>().Property(b => b.LastLogin).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Comment>().Property(b => b.PublishedAt).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Post>().Property(b => b.PublishedAt).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Like>().Property(b => b.PublishedAt).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Comment>().Property(b => b.PublishedAt).HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
