using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternshipsDEIS.Models
{
    public class EvaluateCompany
    {
        public string EvaluateCompanyId { get; set; }

        [ForeignKey("Student")]
        public string StudentId { get; set; }

        public ApplicationUser Student { get; set; }

        [ForeignKey("Company")]
        public string CompanyId { get; set; }

        public ApplicationUser Company { get; set; }

        [Required]
        [Range(0, 20)]
        public int Pontuation { get; set; }

        [Required(ErrorMessage = "You must justify")]
        [DataType(DataType.MultilineText)]
        public string Justification { get; set; }
    }
}