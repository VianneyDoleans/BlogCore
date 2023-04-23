using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DBAccess.Contracts;
using DBAccess.Data.JoiningEntity;
using Microsoft.AspNetCore.Identity;

namespace DBAccess.Data
{

    public class Role : IdentityRole<int>, IPoco, IHasName, IHasUserRoles
    {
        [Required]
        [MaxLength(20)]
        public override string Name { get; set; }

        [ForeignKey("RoleId")]
        public virtual ICollection<UserRole> UserRoles { get; set; }
        
        [ForeignKey("RoleId")]
        public virtual ICollection<DefaultRoles> DefaultRoles { get; set; }
    }
}
