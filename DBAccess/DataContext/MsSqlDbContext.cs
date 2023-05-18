using DBAccess.Data;
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
            builder.Entity<User>().Property(b => b.RegisteredAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");
            builder.Entity<User>().Property(b => b.LastLogin).HasDefaultValueSql("SYSDATETIMEOFFSET()");
            builder.Entity<Comment>().Property(b => b.PublishedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");
            builder.Entity<Post>().Property(b => b.PublishedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");
            builder.Entity<Like>().Property(b => b.PublishedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");
            builder.Entity<Comment>().Property(b => b.PublishedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");
        }
    }
}
