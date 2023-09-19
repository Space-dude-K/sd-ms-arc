using Entities.ModelAttributes;
using Entities.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Forum.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Cabinet { get; set; }
        public string InternalPhone { get; set; }
        public string BirthDate { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
         
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Password and confirmation password not match.")]
        public string ConfirmPassword { get; set; }
        [EnsureMinimumElements(min: 1, ErrorMessage = "Select at least one item")]
        public List<string>? Roles { get; set; }
        public RegisterTableViewModel? RegisterTableViewModel { get; set; }
    }
}