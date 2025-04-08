using System;
using System.ComponentModel.DataAnnotations;

namespace Traditiona_trend_on_rent.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public string OrderId { get; set; }

        public string PaymentId { get; set; }

        public string PaymentStatus { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
