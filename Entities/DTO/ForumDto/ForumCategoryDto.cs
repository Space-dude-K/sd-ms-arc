namespace Entities.DTO.ForumDto
{
    public class ForumCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TotalPosts { get; set; }
        public int TotalForums { get; set; }
        public int TotalTopics { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int ForumUserId { get; set; }
    }
}