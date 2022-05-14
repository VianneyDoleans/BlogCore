using DbAccess.Data.POCO;
using MyBlogAPI.DTO.Contracts;

namespace MyBlogAPI.DTO.Like
{
    /// <summary>
    /// UPDATE Dto type of <see cref="DbAccess.Data.POCO.Like"/>.
    /// </summary>
    public class UpdateLikeDto : ADto, ILikeDto
    {
        public LikeableType LikeableType { get; set; }

        public int? Comment { get; set; }

        public int? Post { get; set; }

        public int User { get; set; }
    }
}
