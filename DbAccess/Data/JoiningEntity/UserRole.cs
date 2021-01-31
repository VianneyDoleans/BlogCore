using DbAccess.Data.Models;

namespace DbAccess.Data.JoiningEntity
{
    public class UserRole
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
