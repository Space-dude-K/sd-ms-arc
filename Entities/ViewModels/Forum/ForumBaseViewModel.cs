using Entities.DTO.ForumDto.ForumView;

namespace Entities.ViewModels.Forum
{
    public class ForumBaseViewModel
    {
        public string ForumTitle { get; set; }
        public List<ForumViewTopicDto> Topics { get; set; }
    }
}