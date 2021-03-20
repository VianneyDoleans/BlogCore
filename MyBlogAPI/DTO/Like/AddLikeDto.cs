using DbAccess.Data.POCO;

namespace MyBlogAPI.DTO.Like
{
    public class AddLikeDto : ILikeDto
    {
        public LikeableType LikeableType { get; set; }

        public int? Comment { get; set; }

        public int? Post { get; set; }

        public int User { get; set; }
    }
}
