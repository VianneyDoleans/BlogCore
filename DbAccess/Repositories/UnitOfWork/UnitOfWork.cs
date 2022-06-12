using DBAccess.DataContext;

namespace DBAccess.Repositories.UnitOfWork
{
    /// <inheritdoc />
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly BlogCoreContext _context;

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
        public UnitOfWork(BlogCoreContext context)
        {
            _context = context;
        }
    }
}
