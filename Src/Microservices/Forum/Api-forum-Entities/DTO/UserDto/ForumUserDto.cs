namespace Entities.DTO.UserDto
{
    public class ForumUserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FirstAndLastNames { get { return FirstName + " " + LastName; } }
        public string UserName { get; set; }
        public string Cabinet { get; set; }
        public string InternalPhone { get; set; }
        public string BirthDate { get; set; }
        public string Company { get; set; }
        public string Division { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? AvatarImgSrc { get; set; }
        public int TotalPostCounter { get; set; }
        public bool IsUserHasAccess { get; set; }
        public DateTime? CreatedAt { get; set; }
        public ICollection<string> Roles { get; set; }
    }
}