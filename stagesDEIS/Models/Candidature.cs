using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace stagesDEIS.Models
{
    public class Candidature
    {
        public string CandidatureId { get; set; }

        [ForeignKey("Candidate")]
        public string StudentId { get; set; }

        public virtual ApplicationUser Candidate { get; set; }

        [ForeignKey("Proposal")]
        public string ProposalId { get; set; }

        public Proposal Proposal { get; set; }

        [Required(ErrorMessage = "You must choose a branch")]
        public Branch Branch { get; set; }

        [Required(ErrorMessage = "You must specify your grades")]
        public string Grades { get; set; }

        [Display(Name = "Unfinished grades")]
        [Required(ErrorMessage = "You must specify your unfinished grades")]
        [DataType(DataType.MultilineText)]
        public string UnfinishedGrades { get; set; }

        public State Result { get; set; }

        public Candidature()
        {
            Result = State.STANDBY;
        }
    }
}