namespace Entities.Models.Forum
{
    public class ForumTopicCounter
    {
        public int Id { get; set; }
        public int? PostCounter { get; set; } = 0;
        public virtual ForumTopic? ForumTopic { get; set; }
        public int? ForumTopicId { get; set; }
    }
}
