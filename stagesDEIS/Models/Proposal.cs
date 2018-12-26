using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stagesDEIS.Models
{
    public class Proposal
    {
        public string ProposalId { get; set; }

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

        [Required]
        [DataType(DataType.MultilineText)]
        public string Location { get; set; }

        [Required]
        public Branch Branch { get; set; }

        [Required(ErrorMessage = "You must provide objectives")]
        [DataType(DataType.MultilineText)]
        [StringLength(200, MinimumLength = 20, ErrorMessage = "Should be at least 20 characters and at most 200")]
        public string Objectives { get; set; }

        public Professor Professor { get; set; }

        [ForeignKey("Professor")]
        public string ProfessorId { get; set; }

        public IList<Student> Candidates { get; set; }

        public Company Company { get; set; }

        [ForeignKey("Company")]
        public string CompanyId { get; set; }

        public Student Placed { get; set; }

        [ForeignKey("Student")]
        public string PlacedId { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Justification must be between 10 and 100 chars")]
        public string Justification { get; set; }

        public Proposal()
        {
            State = State.STANDBY;
            Candidates = new List<Student>();
        }
    }
}