using System;
using DbAccess.Repositories.User;

namespace DbAccess.Repositories.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
    }
}
