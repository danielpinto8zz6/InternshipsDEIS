using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternshipsDEIS.Models
{
    public class Internship
    {
        public string InternshipId { get; set; }

        [Required(ErrorMessage = "You must provide a title")]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [Required(ErrorMessage = "You must provide a description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public State State { get; set; }

        [Display(Name = "Access Conditions")]
        [Required(ErrorMessage = "You must provide access conditions")]
        [DataType(DataType.MultilineText)]
        public string AccessConditions { get; set; }

        [Required(ErrorMessage = "You must specify Internship location")]
        [DataType(DataType.MultilineText)]
        public string Location { get; set; }

        [Required]
        public Branch Branch { get; set; }

        [Required(ErrorMessage = "You must provide objectives")]
        [DataType(DataType.MultilineText)]
        public string Objectives { get; set; }

        public ApplicationUser Advisor { get; set; }

        [Display(Name = "Advisor")]
        [ForeignKey("Professor")]
        public string AdvisorId { get; set; }

        public IList<InternshipCandidature> Candidatures { get; set; }

        public ApplicationUser Company { get; set; }

        [ForeignKey("Company")]
        public string CompanyId { get; set; }

        [DataType(DataType.MultilineText)]
        public string Justification { get; set; }

        public IList<ApplicationUser> Placed { get; set; }

        public Internship()
        {
            State = State.STANDBY;
            Candidatures = new List<InternshipCandidature>();
            Placed = new List<ApplicationUser>();
        }
    }
}