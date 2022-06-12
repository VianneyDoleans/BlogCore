using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DBAccess.Data.POCO.Interface;
using DBAccess.Data.POCO.JoiningEntity;

namespace DBAccess.Data.POCO
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
