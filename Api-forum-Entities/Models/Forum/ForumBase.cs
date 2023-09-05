using System.ComponentModel.DataAnnotations;

namespace Entities.Models.Forum
{
    public class ForumBase
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Forum title is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the ForumTitle is 60 characters.")]
        public string ForumTitle { get; set; }
        [Required(ErrorMessage = "Forum subtitle is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the ForumSubTitle is 60 characters.")]
        public string ForumSubTitle { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public int? TotalPosts { get; set; }
        public int? TotalTopics { get; set; }
        public int TotalViews { get; set; }
        public virtual ForumCategory? ForumCategory { get; set; }
        public int? ForumCategoryId { get; set; }
        public virtual ForumUser? ForumUser { get; set; }
        public int? ForumUserId { get; set; }
        public virtual ICollection<ForumTopic>? ForumTopics { get; set; }
    }
}