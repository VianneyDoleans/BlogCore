using DBAccess.DataContext;

namespace DBAccess.Repositories.UnitOfWork
{
    /// <inheritdoc />
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly BlogCoreContext _coreContext;

        /// <inheritdoc />
        public void Dispose()
        {
            _coreContext.Dispose();
        }

        /// <inheritdoc />
        public void Save()
        {
            _coreContext.SaveChanges();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="coreContext"></param>
        public UnitOfWork(BlogCoreContext context)
        {
            _coreContext = context;
        }
    }
}
