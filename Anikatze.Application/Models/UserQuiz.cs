using System;

namespace Anikatze.Application.Models
{
    public class UserQuiz
    {
        public int UserQuizID { get; set; } // Primary Key
        public string UserQuizGuid { get; set; } = Guid.NewGuid().ToString(); // New Guid field
        public int UserID { get; set; }
        public int QuizID { get; set; }

        public User? User { get; set; } // Nullable
        public Quiz? Quiz { get; set; } // Nullable
    }
}
