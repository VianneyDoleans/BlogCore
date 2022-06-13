using Microsoft.AspNetCore.Identity;

namespace DBAccess.Data.JoiningEntity
{
    public class UserRole : IdentityUserRole<int>
    {
        public virtual User User { get; set; }

        public virtual Role Role { get; set; }
    }
}
