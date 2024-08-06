using System;
using System.Collections.Generic;

namespace AnikatzeDbProject.Model
{
    public class CartHistory
    {
        public int CartHistoryID { get; set; } // Primary Key
        public string CartHistoryGuid { get; set; } = Guid.NewGuid().ToString(); // New Guid field
        public int UserID { get; set; }
        public DateTime ArchivedAt { get; set; } = DateTime.UtcNow;

        public User? User { get; set; } // Nullable
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
