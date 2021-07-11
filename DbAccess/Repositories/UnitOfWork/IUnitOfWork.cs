using System;

namespace DbAccess.Repositories.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
    }
}
