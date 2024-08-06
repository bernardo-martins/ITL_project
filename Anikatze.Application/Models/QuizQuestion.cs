using System.Collections.Generic;

namespace Anikatze.Application.Models
{
    public class QuizQuestion
    {
        public int QuizQuestionID { get; set; } // Primary Key
        public string QuizQuestionGuid { get; set; } = Guid.NewGuid().ToString(); // New Guid field
        public int QuizID { get; set; }
        public string QuestionText { get; set; } = string.Empty;

        public Quiz? Quiz { get; set; } // Nullable
        public ICollection<QuizOption> QuizOptions { get; set; } = new List<QuizOption>();
    }
}
