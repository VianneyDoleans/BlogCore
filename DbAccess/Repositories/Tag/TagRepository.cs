using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DbAccess.DataContext;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Repositories.Tag
{
    public class TagRepository : Repository<Data.POCO.Tag>, ITagRepository
    {
        public TagRepository(MyBlogContext context) : base(context)
        {
        }
    }
}
