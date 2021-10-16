using DbAccess.Data.POCO;

namespace MyBlogAPI.DTO.Like
{
    /// <summary>
    /// Add Dto type of <see cref="DbAccess.Data.POCO.Like"/>.
    /// </summary>
    public class AddLikeDto : ILikeDto
    {
        public LikeableType LikeableType { get; set; }

        public int? Comment { get; set; }

        public int? Post { get; set; }

        public int User { get; set; }
    }
}
