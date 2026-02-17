public class OrderWithUserStatusDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string MobileNumber { get; set; }
    public string Address { get; set; }
    public int NormalQty { get; set; }
    public int CoolQty { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; }

    // ✅ Add these two
    public string BookingType { get; set; }
    public DateOnly? BookingDate { get; set; }

    public string UserActiveStatus { get; set; }
}
