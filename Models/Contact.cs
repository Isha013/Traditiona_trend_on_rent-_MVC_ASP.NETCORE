﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Traditiona_trend_on_rent.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty; // ✅ Default Value

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty; // ✅ Default Value

        [Required(ErrorMessage = "Message is required.")]
        public string Message { get; set; } = string.Empty; // ✅ Default Value

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // ✅ Default to UTC
    }
}
