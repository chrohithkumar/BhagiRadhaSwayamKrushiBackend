namespace BhagiRadhaSwayamKrushi.DTO
{
    public class CreateOrderDto
    {
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public int NormalQty { get; set; }
        public int CoolQty { get; set; }

        public decimal TotalAmount { get; set; }
        public string BookingType { get; set; } // "daily" or "advance"
        public DateOnly? BookingDate { get; set; }
        //
    }
}
