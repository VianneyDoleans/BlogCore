using DbAccess.DataContext;

namespace DbAccess.Repositories.UnitOfWork
{
    /// <inheritdoc />
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyBlogContext _context;

        /// <inheritdoc />
        public void Dispose()
        {
            _context.Dispose();
        }

        /// <inheritdoc />
        public void Save()
        {
            _context.SaveChanges();
        }

        public UnitOfWork(MyBlogContext context)
        {
            _context = context;
        }
    }
}
