using System.ComponentModel.DataAnnotations;

namespace IntershipsDEIS.Models
{
    public class Grade
    {
        public string GradeId { get; set; }

        [Required(ErrorMessage = "You must provide a subject")]
        [DataType(DataType.MultilineText)]
        public string Subject { get; set; }

        [Required(ErrorMessage = "You must provide a pontuation")]
        [Range(0.00, 20.00, ErrorMessage = "Pontuation must be between 0.00 and 20.00")]
        public int Pontuation { get; set; }
    }
}