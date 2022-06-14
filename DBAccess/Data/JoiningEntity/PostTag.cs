namespace DBAccess.Data.JoiningEntity
{
    public class PostTag
    {
        public int PostId { get; set; }
        public virtual Post Post { get; set; }

        public int TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}   
