using System.ComponentModel.DataAnnotations;

namespace stagesDEIS.Models
{
    public class Grade
    {
        public int GradeId { get; set; }
        
        [Required(ErrorMessage = "You must provide a subject")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "You must provide a pontuation")]
        [Range(0.00, 20.00,
            ErrorMessage = "Pontuation must be between 0.00 and 20.00")]
        public int Pontuation { get; set; }

        public Grade(string subject, int pontuation)
        {
            Subject = subject;
            Pontuation = pontuation;
        }
    }
}