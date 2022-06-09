using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DbAccess.Data.POCO.Interface;
using DbAccess.Data.POCO.JoiningEntity;
using Microsoft.AspNetCore.Identity;

namespace DbAccess.Data.POCO
{

    public class Role : IdentityRole<int>, IPoco, IHasName, IHasUserRoles
    {
        [Required]
        [MaxLength(20)]
        public override string Name { get; set; }

        [ForeignKey("RoleId")]
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
