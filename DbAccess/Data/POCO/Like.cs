using System;
using System.ComponentModel.DataAnnotations;
using DbAccess.Data.POCO.Interface;

namespace DbAccess.Data.POCO
{
    public enum LikeableType : byte
    {
        Comment = 0,
        Post = 1,
    }

    public class Like : IPoco, IHasCreationDate
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime PublishedAt { get; set; }

        [Required]
        public LikeableType LikeableType { get; set; }

        public Comment Comment { get; set; }

        public Post Post { get; set; }

        [Required]
        public User User { get; set; }
    }
}
