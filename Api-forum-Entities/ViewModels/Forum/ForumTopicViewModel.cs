using Entities.DTO.ForumDto;
using Entities.DTO.ForumDto.ForumView;

namespace Entities.ViewModels.Forum
{
    public class ForumTopicViewModel
    {
        public int TotalPosts { get; set; }
        public int TotalPages { get { return (int)Math.Ceiling((decimal)TotalPosts / 4); } }
        public int TopicId { get; set; }
        public string PostText { get; set; }
        public string SubTopicAuthor { get; set; }
        public string SubTopicCreatedAt { get; set; }
        public List<ForumViewPostDto> Posts { get; set; }
    }
}