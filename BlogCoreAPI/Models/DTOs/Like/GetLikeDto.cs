using System;
using DBAccess.Data;

namespace BlogCoreAPI.Models.DTOs.Like
{
    /// <summary>
    /// GET Dto type of <see cref="Like"/>.
    /// </summary>
    public class GetLikeDto : ADto, ILikeDto
    {
        public DateTimeOffset PublishedAt { get; set; }

        public LikeableType LikeableType { get; set; }

        public int? Comment { get; set; }

        public int? Post { get; set; }

        public int User { get; set; }
    }
}
