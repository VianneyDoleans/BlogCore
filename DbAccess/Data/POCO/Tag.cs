using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DbAccess.Data.POCO.Interface;
using DbAccess.Data.POCO.JoiningEntity;

namespace DbAccess.Data.POCO
{
    public class Tag : IPoco, IHasName
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
