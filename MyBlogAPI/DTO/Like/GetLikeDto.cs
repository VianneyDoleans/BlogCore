using System;
using DbAccess.Data.POCO;
using MyBlogAPI.DTO.Contracts;

namespace MyBlogAPI.DTO.Like
{
    /// <summary>
    /// GET Dto type of <see cref="DbAccess.Data.POCO.Like"/>.
    /// </summary>
    public class GetLikeDto : ADto, ILikeDto
    {
        public DateTime PublishedAt { get; set; }

        public LikeableType LikeableType { get; set; }

        public int? Comment { get; set; }

        public int? Post { get; set; }

        public int User { get; set; }
    }
}
