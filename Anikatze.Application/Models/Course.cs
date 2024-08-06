using System;
using System.Collections.Generic;

namespace Anikatze.Application.Models
{
    public class Course
    {
        public int CourseID { get; set; } // Primary Key
        public string CourseGuid { get; set; } = Guid.NewGuid().ToString(); // New Guid field
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();
    }
}
