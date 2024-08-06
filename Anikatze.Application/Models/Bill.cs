using System;
using System.Collections.Generic;

namespace Anikatze.Application.Models
{
    public class Bill
    {
        public int BillID { get; set; } // Primary Key
        public string BillGuid { get; set; } = Guid.NewGuid().ToString(); // New Guid field
        public int UserID { get; set; }
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

        public User? User { get; set; } // Nullable
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
