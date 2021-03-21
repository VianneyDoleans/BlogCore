using System;
using DbAccess.Data.POCO;

namespace MyBlogAPI.DTO.Like
{
    public class GetLikeDto : ADto, ILikeDto
    {
        public DateTime PublishedAt { get; set; }

        public LikeableType LikeableType { get; set; }

        public int? Comment { get; set; }

        public int? Post { get; set; }

        public int User { get; set; }
    }
}
