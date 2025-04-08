using System.ComponentModel.DataAnnotations;

namespace Traditiona_trend_on_rent.Models
{
    public class UserProfile
    {
        public string Name { get; set; } = "Umang Itank";
        public string Role { get; set; } = "User";
        public string Email { get; set; } = "umangitank99@gmail.com";
        public string Mobile { get; set; } = "9173914174";
        public string? PhotoPath { get; set; }
        public string Password { get; set; } = "123456";
    }
}
