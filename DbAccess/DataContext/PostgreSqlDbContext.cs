using DBAccess.Data.POCO;
using Microsoft.EntityFrameworkCore;

namespace DBAccess.DataContext
{
    public class PostgreSqlDbContext : MyBlogContext
    {
        /// <summary>
        /// Context used for PostgreSQL database (compatibility)
        /// </summary>
        public PostgreSqlDbContext(DbContextOptions options) : base(options)
        {
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().Property(b => b.RegisteredAt).HasDefaultValueSql("NOW()");
            builder.Entity<User>().Property(b => b.LastLogin).HasDefaultValueSql("NOW()");
            builder.Entity<Comment>().Property(b => b.PublishedAt).HasDefaultValueSql("NOW()");
            builder.Entity<Post>().Property(b => b.PublishedAt).HasDefaultValueSql("NOW()");
            builder.Entity<Like>().Property(b => b.PublishedAt).HasDefaultValueSql("NOW()");
            builder.Entity<Comment>().Property(b => b.PublishedAt).HasDefaultValueSql("NOW()");
        }
    }
}
