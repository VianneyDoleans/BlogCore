using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DBAccess.Data.POCO.Interface;

namespace DBAccess.Data.POCO
{
    public class Category : IPoco, IHasName, IHasPosts
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [ForeignKey("CategoryId")]
        public virtual ICollection<Post> Posts { get; set; }
    }
}
