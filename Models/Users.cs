using System.ComponentModel.DataAnnotations;

namespace Traditiona_trend_on_rent.Models
{
    public class Users
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; } = string.Empty; // ✅ Default value to prevent null

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty; // ✅ Default value

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Enter a valid 10-digit phone number")]
        public string Phone { get; set; } = string.Empty; // ✅ Default value

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty; // ✅ Default value
    }
}
