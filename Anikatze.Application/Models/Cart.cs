using System;
using System.Collections.Generic;

namespace Anikatze.Application.Models
{
    public class Cart
    {
        public int CartID { get; set; } // Primary Key
        public string CartGuid { get; set; } = Guid.NewGuid().ToString(); // New Guid field
        public int UserID { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User? User { get; set; } // Nullable
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
