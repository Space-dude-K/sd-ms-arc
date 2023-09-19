using Entities.Models.Forum;
using Entities.DTO.UserDto;
using Entities.Models.File;
using Entities.DTO.FileDto;

namespace Entities.DTO.ForumDto
{
    public class ForumPostDto
    {
        public int Id { get; set; }
        public string PostText { get; set; }
        public int Likes { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string PostDate { get { return CreatedAt.Value.ToShortDateString(); } }
        public DateTime? UpdatedAt { get; set; }
        public int ForumTopicId { get; set; }
        public int ForumUserId { get; set; }
        public ForumUserDto ForumUser { get; set; }
        public List<ForumFileDto> ForumFiles { get; set; }
    }
}