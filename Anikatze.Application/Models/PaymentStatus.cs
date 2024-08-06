namespace Anikatze.Application.Models
{
    public class PaymentStatus
    {
        public int PaymentStatusID { get; set; } // Primary Key
        public string PaymentStatusGuid { get; set; } = Guid.NewGuid().ToString(); // New Guid field
        public string Status { get; set; } = string.Empty;

        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
