using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DBAccess.Contracts;
using DBAccess.Data.JoiningEntity;

namespace DBAccess.Data
{
    public class Post : IPoco, IHasContent, IHasName, IHasAuthor, IHasCategory, IHasCreationDate, IHasModificationDate, IHasPostTag, IHasLikes, IHasComments
    {
        [Key]
        public int Id { get; set; }

        [Required] public string Content { get; set; }

        [Required] [MaxLength(250)] public string Name { get; set; }

        [Required] public User Author { get; set; }

        [Required] public Category Category { get; set; }

        [Required]
        public DateTimeOffset PublishedAt { get; set; }

        public DateTimeOffset? ModifiedAt { get; set; }

        [ForeignKey("PostId")]
        public virtual ICollection<PostTag> PostTags { get; set; }

        [ForeignKey("PostId")]
        public virtual ICollection<Like> Likes { get; set; }

        [ForeignKey("PostId")]
        public virtual ICollection<Comment> Comments { get; set; }

        public string ThumbnailUrl { get; set; }
    }
}
