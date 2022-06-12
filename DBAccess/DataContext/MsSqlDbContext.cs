using DBAccess.Data.POCO;
using Microsoft.EntityFrameworkCore;

namespace DBAccess.DataContext
{
    /// <summary>
    /// context used for Microsoft SQL Server database (compatibility)
    /// </summary>
    public class MsSqlDbContext : BlogCoreContext
    {
        /// <inheritdoc />
        public MsSqlDbContext(DbContextOptions options) : base(options) { }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().Property(b => b.RegisteredAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Entity<User>().Property(b => b.LastLogin).HasDefaultValueSql("GETUTCDATE()");
            builder.Entity<Comment>().Property(b => b.PublishedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Entity<Post>().Property(b => b.PublishedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Entity<Like>().Property(b => b.PublishedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Entity<Comment>().Property(b => b.PublishedAt).HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
