using Entities.DTO.ForumDto.Create;
using System.ComponentModel.DataAnnotations;

namespace Entities.DTO.ForumDto.Manipulation
{
    public abstract class ForumBaseForManipulationDto
    {
        [Required(ErrorMessage = "Forum title is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the forum title is 30 characters.")]
        public string ForumTitle { get; set; }
        public int ForumUserId { get; set; }
        [Required(ErrorMessage = "Forum sub title is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the forum sub title is 30 characters.")]
        public string ForumSubTitle { get; set; }
        public int TotalViews { get; set; }
    }
}
