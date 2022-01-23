using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using DbAccess.Data.Models.Permission;
using DbAccess.Data.POCO.Interface;
using DbAccess.Data.POCO.JoiningEntity;

namespace DbAccess.Data.POCO
{

    public class Role : IPoco, IHasName, IHasUserRoles
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [ForeignKey("RoleId")]
        public virtual ICollection<UserRole> UserRoles { get; set; }

        public string Permissions { get; set; }

        [NotMapped]
        public List<Permission> PermissionAccess
        {
            get => JsonSerializer.Deserialize<List<Permission>>(Permissions);
            set => Permissions = JsonSerializer.Serialize(value);
        }
    }
}
