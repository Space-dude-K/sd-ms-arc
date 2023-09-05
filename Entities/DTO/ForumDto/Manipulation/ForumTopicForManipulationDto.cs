using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.ForumDto.Manipulation
{
    public abstract class ForumTopicForManipulationDto
    {
        [Required(ErrorMessage = "Topic name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the topic name is 30 characters.")]
        public string Name { get; set; }
        public int ForumUserId { get; set; }
        public int TotalViews { get; set; }
    }
}
