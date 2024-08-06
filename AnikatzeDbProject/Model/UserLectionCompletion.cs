using System;

//Niklas hinzugefügt

namespace AnikatzeDbProject.Model
{
    public class UserLectionCompletion
    {
        public int UserLectionCompletionID { get; set; } // Primary Key 
        public int UserID { get; set; }
        public int LectionID { get; set; }
        public DateTime CompletionDate { get; set; } // Wann die Lektion abgeschlossen wurde
        public TimeSpan TimeSpent { get; set; } // Wie lange der Benutzer in der Lektion war
        public string UserLectionCompletionGuid { get; set; } = Guid.NewGuid().ToString(); // New Guid field

        public User? User { get; set; } 
        public Lection? Lection { get; set; } 
    }
}