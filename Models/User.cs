using System.ComponentModel.DataAnnotations;

namespace BhagiRadhaSwayamKrushi.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mobile Number is required")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter a valid 10-digit mobile number")]
        public string Mobile { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; } = string.Empty;

        public string Role { get; set; } = "User";

        // ActiveStatus: "Active" or "Blocked"
        public string ActiveStatus { get; set; } = "Active";
    }
}


