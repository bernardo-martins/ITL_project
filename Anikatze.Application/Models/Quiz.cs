using System;
using System.Collections.Generic;

namespace Anikatze.Application.Models
{
    public class Quiz
    {
        public int QuizID { get; set; } // Primary Key
        public string QuizGuid { get; set; } = Guid.NewGuid().ToString(); // New Guid field
        public string Title { get; set; } = string.Empty;
        public int LectionID { get; set; }

        public Lection? Lection { get; set; } // Nullable
        public ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();
        public ICollection<UserQuiz> UserQuizzes { get; set; } = new List<UserQuiz>();
    }
}
