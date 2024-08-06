using System;

namespace Anikatze.Application.Models
{
    public class Lection
    {
        public int LectionID { get; set; } // Primary Key
        public string LectionGuid { get; set; } = Guid.NewGuid().ToString(); // New Guid field
        public string Title { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty; // New field
        public int CourseID { get; internal set; }

        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
        public ICollection<Video> Videos { get; set; } = new List<Video>();
        public ICollection<UserLectionCompletion> UserLectionCompletions { get; set; } = new List<UserLectionCompletion>();

    }
}
