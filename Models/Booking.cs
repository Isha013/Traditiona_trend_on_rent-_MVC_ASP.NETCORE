using System.ComponentModel.DataAnnotations;

namespace Traditiona_trend_on_rent.Models
{
    public class Booking

    {

        [Required]
        public int id { get; set; } // Add ID

        public string Name { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string CholiName { get; set; } = string.Empty;

        [Required]
        public string CholiType { get; set; } = string.Empty;

        [Required]
        public string TopwearFabric { get; set; } = string.Empty;

        [Required]
        public string BottomwearFabric { get; set; } = string.Empty;

        [Required]
        public string DupattaType { get; set; } = string.Empty;

        [Required]
        public decimal RentalPrice { get; set; } = 0m;

        [Required]
        public string SetType { get; set; } = string.Empty;

        [Required]
        public string RentalTime { get; set; } = string.Empty;

        [Required]
        public string SetSize { get; set; } = string.Empty;

        [Required]
        public string ContactNumber { get; set; } = string.Empty;
    }
}
