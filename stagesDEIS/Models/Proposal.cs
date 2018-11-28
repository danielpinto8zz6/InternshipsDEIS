using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stagesDEIS.Models
{
    public class Proposal
    {
        public int ProposalId { get; set; }

        [Required(ErrorMessage = "You must provide a title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "You must provide a description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{e:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public State State { get; set; }

        [Required(ErrorMessage = "You must provide access conditions")]
        public string AccessConditions { get; set; }

        public Branch Branch { get; set; }

        [Required(ErrorMessage = "You must provide objectives")]
        [DataType(DataType.MultilineText)]
        public string Objectives { get; set; }

        public Professor Professor { get; set; }

        [ForeignKey("Professor")]
        public int ProfessorId { get; set; }

        public IList<Student> Candidates { get; set; }

        public Company Company { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }

        public Student Placed { get; set; }

        [ForeignKey("Student")]
        public int PlacedId { get; set; }
    }
}