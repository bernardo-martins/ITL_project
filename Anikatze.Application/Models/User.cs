using System;
using System.Collections.Generic;

namespace Anikatze.Application.Models
{
    public class User
    {
        public int UserID { get; set; } // Primary Key
        public string UserGuid { get; set; } = Guid.NewGuid().ToString(); // New Guid field
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty; // New field
        public string LastName { get; set; } = string.Empty; // New field

        public ICollection<Bill> Bills { get; set; } = new List<Bill>();
        public Cart? Cart { get; set; } // Nullable
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<UserQuiz> UserQuizzes { get; set; } = new List<UserQuiz>();
        public ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();
        public ICollection<UserLectionCompletion> UserLectionCompletions { get; set; } = new List<UserLectionCompletion>();

    }
}
