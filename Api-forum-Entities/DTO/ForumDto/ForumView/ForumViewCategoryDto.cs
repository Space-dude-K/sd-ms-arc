namespace Entities.DTO.ForumDto.ForumView
{
    public class ForumViewCategoryDto : ForumCategoryDto
    {
        public List<ForumViewBaseDto> Forums { get; set; }
    }
}