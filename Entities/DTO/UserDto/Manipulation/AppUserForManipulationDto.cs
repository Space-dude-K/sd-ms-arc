namespace Entities.DTO.UserDto.Manipulation
{
    public abstract class AppUserForManipulationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Cabinet { get; set; }
        public int InternalPhone { get; set; }
        public string BirthDate { get; set; }
        public string Division { get; set; }
        public string Company { get; set; }
    }
}