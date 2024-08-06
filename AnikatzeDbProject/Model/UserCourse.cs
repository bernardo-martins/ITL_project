using System;

namespace AnikatzeDbProject.Model
{
    public class UserCourse
    {
        public int UserID { get; set; }
        public int CourseID { get; set; }
        public string UserCourseGuid { get; set; } = Guid.NewGuid().ToString(); // New Guid field

        public User? User { get; set; } // Nullable
        public Course? Course { get; set; } // Nullable
    }
}
