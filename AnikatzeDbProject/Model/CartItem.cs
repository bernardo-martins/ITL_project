using System;

namespace AnikatzeDbProject.Model
{
    public class CartItem
    {
        public int CartItemID { get; set; } // Primary Key
        public string CartItemGuid { get; set; } = Guid.NewGuid().ToString(); // New Guid field
        public int CartID { get; set; }
        public int? CartHistoryID { get; set; }
        public int CourseID { get; set; }
        public int Quantity { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        public Cart? Cart { get; set; } // Nullable
        public CartHistory? CartHistory { get; set; } // Nullable
        public Course? Course { get; set; } // Nullable
    }
}
