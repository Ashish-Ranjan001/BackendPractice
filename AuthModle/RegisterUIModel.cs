using System.ComponentModel.DataAnnotations;

namespace BackendPractice.AuthModle
{
    public class RegisterUIModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Age { get; set; }
        public string PhoneNumber { get; set; }= string.Empty;

    }
}
