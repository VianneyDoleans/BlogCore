using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
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

        /*public string Permissions { get; set; }

        [NotMapped]
        public List<Permission> PermissionAccess
        {
            get => JsonSerializer.Deserialize<List<Permission>>(Permissions);
            set => Permissions = JsonSerializer.Serialize(value);
        }*/
    }
}
