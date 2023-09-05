using Entities.Models.File;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models.Forum
{
    public class ForumUser
    {
        public int Id { get; set; }
        public string SimplifiedName { get; set; }
        public int? Karma { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public virtual ForumAccount ForumAccount { get; set; }
        public int? TotalPostCounter { get; set; }
        public string AvatarImgSrc { get; set; }
        public virtual AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
        public virtual ICollection<ForumFile> ForumFiles { get; set; }
    }
}