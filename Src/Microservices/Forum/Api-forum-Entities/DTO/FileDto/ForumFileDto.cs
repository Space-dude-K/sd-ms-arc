namespace Entities.DTO.FileDto
{
    public class ForumFileDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int ForumPostId { get; set; }
        public int ForumUserId { get; set; }
    }
}