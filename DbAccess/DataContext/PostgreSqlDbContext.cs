using DbAccess.Data.POCO;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.DataContext
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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().Property(b => b.RegisteredAt).HasDefaultValueSql("NOW()");
            modelBuilder.Entity<User>().Property(b => b.LastLogin).HasDefaultValueSql("NOW()");
            modelBuilder.Entity<Comment>().Property(b => b.PublishedAt).HasDefaultValueSql("NOW()");
            modelBuilder.Entity<Post>().Property(b => b.PublishedAt).HasDefaultValueSql("NOW()");
            modelBuilder.Entity<Like>().Property(b => b.PublishedAt).HasDefaultValueSql("NOW()");
            modelBuilder.Entity<Comment>().Property(b => b.PublishedAt).HasDefaultValueSql("NOW()");
        }
    }
}
