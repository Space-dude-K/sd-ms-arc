namespace Entities.DTO.ForumDto.Manipulation
{
    public abstract class ForumPostForManipulationDto
    {
        public int ForumTopicId { get; set; }
        public int ForumUserId { get; set; }
        public string PostName { get; set; } = "TestName";
        public string PostText { get; set; }
        public int Likes { get; set; } = 0;
    }
}