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

        // Nullable latitude and longitude
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public int NormalQty { get; set; }
        public int CoolQty { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
    }
}

