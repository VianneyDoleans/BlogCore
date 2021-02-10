using System.Collections.Generic;

namespace MyBlogAPI.DTO
{
    public class Role
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public virtual ICollection<DbAccess.Data.POCO.User> Users { get; set; }
    }
}
