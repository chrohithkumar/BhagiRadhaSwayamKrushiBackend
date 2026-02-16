using System.ComponentModel.DataAnnotations;

namespace BhagiRadhaSwayamKrushi.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string MobileNumber { get; set; }

        public string Address { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public int NormalQty { get; set; }
        public int CoolQty { get; set; }

        public decimal TotalAmount { get; set; }

        // --- UPDATED FIELDS ---
        [Required]
        public string BookingType { get; set; } = "daily"; // values: "daily", "advance"

        public DateTime BookingDate { get; set; } // The date for delivery
        // ----------------------

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
    }
}

