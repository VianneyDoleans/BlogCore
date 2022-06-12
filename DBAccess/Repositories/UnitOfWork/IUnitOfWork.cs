using System;

namespace DBAccess.Repositories.UnitOfWork
{
    /// <summary>
    /// Interface used to define the "Unit of Work", the element necessary to manage the transaction on the database.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Method used to save the modifications on the database.
        /// All changes made will be commit to the database.
        /// </summary>
        void Save();
    }
}
