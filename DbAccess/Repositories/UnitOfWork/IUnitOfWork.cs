using System;
using System.Collections.Generic;
using System.Text;

namespace DbAccess.Repositories.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        void Save();
    }
}
