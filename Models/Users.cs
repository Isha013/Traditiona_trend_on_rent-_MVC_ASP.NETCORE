using System;
using System.ComponentModel.DataAnnotations;

namespace Traditiona_trend_on_rent.Models
{
    public class Users
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; } = string.Empty; // ✅ Default Value

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty; // ✅ Default Value

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string Phone { get; set; } = string.Empty; // ✅ Default Value

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; } = string.Empty; // ✅ Default Value

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // ✅ Default to UTC
    }
}
