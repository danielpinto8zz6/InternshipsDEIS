using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternshipsDEIS.Models
{
    public class EvaluateStudent
    {
        public string EvaluateStudentId { get; set; }

        // Entity, could be a professor or company
        [ForeignKey("Entity")]
        public string EntityId { get; set; }

        public ApplicationUser Entity { get; set; }

        [Display(Name = "Student")]
        [ForeignKey("Student")]
        public string StudentId { get; set; }

        public ApplicationUser Student { get; set; }

        [Required]
        [Range(0, 20)]
        public int Pontuation { get; set; }

        [Required(ErrorMessage = "You must justify")]
        [DataType(DataType.MultilineText)]
        public string Justification { get; set; }
    }
}