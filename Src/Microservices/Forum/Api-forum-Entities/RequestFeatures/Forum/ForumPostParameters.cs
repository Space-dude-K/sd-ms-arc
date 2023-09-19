namespace Entities.RequestFeatures.Forum
{
    public class ForumPostParameters : RequestParameters
    {
        public ForumPostParameters()
        {
            OrderBy = "PostName";
        }

        public uint MinLikes { get; set; }
        public uint MaxLikes { get; set; } = int.MaxValue;
        public bool ValidLikeRange => MaxLikes > MinLikes;
        public int UserId { get; set; }
    }
}