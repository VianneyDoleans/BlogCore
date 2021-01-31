using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DbAccess.Data.JoiningEntity;

namespace DbAccess.Data.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [ForeignKey("TagId")]
        public virtual ICollection<PostTag> PostTags { get; set; }

        /*public Tag()
        {
            ArticleTags = new HashSet<ArticleTag>();
        }*/
    }
}
