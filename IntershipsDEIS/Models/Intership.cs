using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntershipsDEIS.Models
{
    public class Intership
    {
        public string IntershipId { get; set; }

        [Required(ErrorMessage = "You must provide a title")]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [Required(ErrorMessage = "You must provide a description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public State State { get; set; }

        [Required(ErrorMessage = "You must provide access conditions")]
        [DataType(DataType.MultilineText)]
        public string AccessConditions { get; set; }

        [Required(ErrorMessage = "You must specify Intership location")]
        [DataType(DataType.MultilineText)]
        public string Location { get; set; }

        [Required]
        public Branch Branch { get; set; }

        [Required(ErrorMessage = "You must provide objectives")]
        [DataType(DataType.MultilineText)]
        public string Objectives { get; set; }

        public Professor Advisor { get; set; }

        [Display(Name = "Advisor")]
        [ForeignKey("Professor")]
        public string AdvisorId { get; set; }

        public IList<IntershipCandidature> Candidatures { get; set; }

        public Company Company { get; set; }

        [ForeignKey("Company")]
        public string CompanyId { get; set; }

        [DataType(DataType.MultilineText)]
        public string Justification { get; set; }

        public Intership()
        {
            State = State.ACCEPTED;
            Candidatures = new List<IntershipCandidature>();
        }
    }
}