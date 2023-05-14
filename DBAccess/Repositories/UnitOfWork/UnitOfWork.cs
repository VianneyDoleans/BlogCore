using System;
using System.Linq;
using DBAccess.Contracts;
using DBAccess.DataContext;
using Microsoft.EntityFrameworkCore;

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

        
        private void ChangeModifiedAt()
        {
            var added = _context.ChangeTracker.Entries()
                .Where(t => t.Entity is IHasModificationDate && t.State == EntityState.Added)
                .Select(t => t.Entity)
                .ToArray();
            
            foreach (var entity in added)
            {
                if (entity is IHasModificationDate track)
                {
                    track.ModifiedAt = DateTimeOffset.UtcNow;
                }
            }
        }
        
        private void ChangeLastLogin()
        {
            var added = _context.ChangeTracker.Entries()
                .Where(t => t.Entity is IHasLastLogin && t.State == EntityState.Added)
                .Select(t => t.Entity)
                .ToArray();
            
            foreach (var entity in added)
            {
                if (entity is IHasLastLogin track)
                {
                    track.LastLogin = DateTimeOffset.UtcNow;
                }
            }
        }
        
        /// <inheritdoc />
        public void Save()
        {
            ChangeModifiedAt();
            ChangeLastLogin();

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
