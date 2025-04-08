    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations;

    namespace Traditiona_trend_on_rent.Models
    {
        public class Collection
        {
            public int Id { get; set; }

            [Required]
            public string CholiName { get; set; }

            [Required]
            public string CholiType { get; set; }

            [Required]
            public string TopwearFabric { get; set; }

            [Required]
            public string BottomwearFabric { get; set; }

            [Required]
            public string DupattaType { get; set; }

            [Required]
            public decimal RentalPrice { get; set; }

            [Required]
            public string SetType { get; set; }

            [Required]
            public string RentalDuration { get; set; }

            [Required]
            public string SetSize { get; set; }

            public string CholiImage { get; set; }  // This stores the image file name

            public IFormFile CholiImageFile { get; set; }  // This handles file upload
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // ✅ Default to UTC

        }
    }
