using System;
using System.Collections.Generic;
using System.Text;
using DbAccess.DataContext;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private MyBlogContext _context;

        public IUserRepository Users { get; }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public UnitOfWork(MyBlogContext context)
        {
            _context = context;
            Users = new UserRepository(context);
        }
    }
}
