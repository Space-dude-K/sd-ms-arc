using Entities.Models.Forum;

namespace Entities.Models.File
{
    public class ForumFile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public virtual ForumUser? ForumUser { get; set; }
        public int? ForumUserId { get; set; }
        public virtual ForumPost? ForumPost { get; set; }
        public int? ForumPostId { get; set; }
    }
}