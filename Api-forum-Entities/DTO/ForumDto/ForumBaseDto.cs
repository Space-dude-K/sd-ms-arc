namespace Entities.DTO.ForumDto
{
    public class ForumBaseDto
    {
        public int Id { get; set; }
        public string ForumTitle { get; set; }
        public string ForumSubTitle { get; set; }
        public int TotalViews { get;set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int ForumCategoryId { get; set; }
        public int ForumUserId { get; set; }
    }
}