using Microsoft.AspNetCore.Mvc.Rendering;

namespace Entities.ViewModels.Forum
{
    public class ForumBaseCreationView
    {
        public List<SelectListItem>? Categories { get; set; }
        public int SelectedCategoryId { get; set; }
        public string ForumTitle { get; set; }
        public string ForumSubtitle { get; set;}
    }
}