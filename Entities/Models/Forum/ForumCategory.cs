using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models.Forum
{
    public class ForumCategory
    {
        private int totalPost;

        public int Id { get; set; }
        [Required(ErrorMessage = "Category title is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the category title is 60 characters.")]
        public string Name { get; set; }
        public int TotalTopics { get; set; }
        public int TotalForums { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public virtual ForumUser? ForumUser { get; set; }
        public int? ForumUserId { get; set; }
        public virtual ICollection<ForumBase> ForumBases { get; set; }
    }
}
