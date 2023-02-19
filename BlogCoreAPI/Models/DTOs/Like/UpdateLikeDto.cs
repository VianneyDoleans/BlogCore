using DBAccess.Data;

namespace BlogCoreAPI.Models.DTOs.Like
{
    /// <summary>
    /// UPDATE Dto type of <see cref="Like"/>.
    /// </summary>
    public class UpdateLikeDto : ADto, ILikeDto
    {
        public LikeableType LikeableType { get; set; }

        public int? Comment { get; set; }

        public int? Post { get; set; }

        public int User { get; set; }
    }
}
