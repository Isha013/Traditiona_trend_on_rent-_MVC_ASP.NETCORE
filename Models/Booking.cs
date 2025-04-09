namespace Traditiona_trend_on_rent.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string CholiName { get; set; }
        public string CholiType { get; set; }
        public string TopwearFabric { get; set; }
        public string BottomwearFabric { get; set; }
        public string DupattaType { get; set; }
        public decimal RentalPrice { get; set; }
        public string SetType { get; set; }
        public string RentalTime { get; set; }
        public string SetSize { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; } // Now added correctly
    }
}
