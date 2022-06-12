using System;
using DBAccess.Data.POCO;

namespace BlogCoreAPI.DTOs.Like
{
    /// <summary>
    /// GET Dto type of <see cref="Like"/>.
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
