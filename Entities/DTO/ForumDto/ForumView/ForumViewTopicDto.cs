namespace Entities.DTO.ForumDto.ForumView
{
    public class ForumViewTopicDto : ForumTopicDto
    {
        public int TotalPosts { get; set; }
        public int TotalViews { get; set; }
    }
}