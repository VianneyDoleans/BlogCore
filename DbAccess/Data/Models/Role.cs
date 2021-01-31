using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DbAccess.Data.JoiningEntity;

namespace DbAccess.Data.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [ForeignKey("RoleId")]
        public virtual ICollection<UserRole> UserRoles { get; set; }

        /*public Role()
        {
            UserRoles = new HashSet<UserRole>();
        }*/
    }
}
