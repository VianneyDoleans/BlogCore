using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DBAccess.Contracts;

namespace DBAccess.Data
{
    public class Comment : IPoco, IHasAuthor, IHasCreationDate, IHasModificationDate, IHasContent, IHasCommentParent, IHasPostParent, IHasLikes, IHasChildrenComments
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public User Author { get; set; }    

        [Required]
        public Post PostParent { get; set; }

        public Comment CommentParent { get; set; }

        [Required]
        public DateTime PublishedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        [Required]
        public string Content { get; set; }

        [ForeignKey("CommentId")]
        public ICollection<Like> Likes { get; set; }

        [ForeignKey("CommentParentId")]
        public ICollection<Comment> ChildrenComments { get; set; }
    }
}
