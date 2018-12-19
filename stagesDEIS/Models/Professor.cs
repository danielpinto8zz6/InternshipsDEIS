using System.ComponentModel.DataAnnotations;

namespace stagesDEIS.Models
{
    public class Professor
    {
        public string ProfessorId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        public InternshipCommittee committee;
        
        [Required(ErrorMessage = "You must provide a phone number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string Contact { get; set; }
    }
}