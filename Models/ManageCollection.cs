using System.ComponentModel.DataAnnotations.Schema;

namespace Traditiona_trend_on_rent.Models
{
    public class ManageCollection
    {
        public int Id { get; set; }
        public string CholiName { get; set; }
        public string CholiType { get; set; }
        public string TopwearFabric { get; set; }
        public string BottomwearFabric { get; set; }
        public string DupattaType { get; set; }
        public decimal RentalPrice { get; set; }
        public string SetType { get; set; }
        public string RentalDuration { get; set; }
        public string SetSize { get; set; }
        public string? CholiImage { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }


        public DateTime CreatedAt { get; set; }
    }
}
