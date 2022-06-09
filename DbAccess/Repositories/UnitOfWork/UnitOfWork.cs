using DbAccess.DataContext;

namespace DbAccess.Repositories.UnitOfWork
{
    /// <inheritdoc />
    public sealed class UnitOfWork : IUnitOfWork
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

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="context"></param>
        public UnitOfWork(MyBlogContext context)
        {
            _context = context;
        }
    }
}
