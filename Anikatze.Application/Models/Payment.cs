using System;

namespace Anikatze.Application.Models
{
    public class Payment
    {
        public int PaymentID { get; set; } // Primary Key
        public string PaymentGuid { get; set; } = Guid.NewGuid().ToString(); // New Guid field
        public int BillID { get; set; }
        public int PaymentStatusID { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaidAt { get; set; } = DateTime.UtcNow;

        public Bill? Bill { get; set; } // Nullable
        public PaymentStatus? PaymentStatus { get; set; } // Nullable
    }
}
