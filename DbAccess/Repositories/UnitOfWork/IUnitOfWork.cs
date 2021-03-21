using System;
using DbAccess.Repositories.User;

namespace DbAccess.Repositories.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        void Save();
    }
}
