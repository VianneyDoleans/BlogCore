using System;
using System.ComponentModel.DataAnnotations;
using DBAccess.Contracts;

namespace DBAccess.Data
{
    public enum LikeableType : byte
    {
        Comment = 0,
        Post = 1,
    }

    public class Like : IPoco, IHasCreationDate, IHasComment, IHasPost, IHasUser
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public DateTimeOffset PublishedAt { get; set; }

        [Required]
        public LikeableType LikeableType { get; set; }

        public Comment Comment { get; set; }

        public Post Post { get; set; }

        [Required]
        public User User { get; set; }
    }
}
