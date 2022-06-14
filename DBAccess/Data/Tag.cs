using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DBAccess.Contracts;
using DBAccess.Data.JoiningEntity;

namespace DBAccess.Data
{
    public class Tag : IPoco, IHasName, IHasPostTag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [ForeignKey("TagId")]
        public virtual ICollection<PostTag> PostTags { get; set; }
    }
}
