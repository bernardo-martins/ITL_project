namespace Anikatze.Application.Models
{
    public class QuizOption
    {
        public int QuizOptionID { get; set; } // Primary Key
        public string QuizOptionGuid { get; set; } = Guid.NewGuid().ToString(); // New Guid field
        public int QuizQuestionID { get; set; }
        public string OptionText { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }

        public QuizQuestion? QuizQuestion { get; set; } // Nullable
    }
}
