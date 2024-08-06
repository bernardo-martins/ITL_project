using System;
using System.Collections.Generic;

namespace AnikatzeDbProject.Model
{
    public class Review
    {
        public int ReviewID { get; set; } // Primary Key
        public string ReviewGuid { get; set; } = Guid.NewGuid().ToString(); // New Guid field
        public int UserID { get; set; }
        public int CourseID { get; set; }
        public string Content { get; set; } = string.Empty;
        public int Rating { get; set; }

        public User? User { get; set; } // Nullable
        public Course? Course { get; set; } // Nullable
    }
}
