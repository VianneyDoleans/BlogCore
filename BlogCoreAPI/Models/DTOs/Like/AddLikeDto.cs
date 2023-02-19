using DBAccess.Data;

namespace BlogCoreAPI.Models.DTOs.Like
{
    /// <summary>
    /// Add Dto type of <see cref="Like"/>.
    /// </summary>
    public class AddLikeDto : ILikeDto
    {
        public LikeableType LikeableType { get; set; }

        public int? Comment { get; set; }

        public int? Post { get; set; }

        public int User { get; set; }
    }
}
