using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace IntershipsDEIS.Models
{
    public class IntershipCandidature
    {
        public string IntershipCandidatureId { get; set; }

        [ForeignKey("Candidate")]
        public string CandidateId { get; set; }

        public virtual ApplicationUser Candidate { get; set; }

        [ForeignKey("Intership")]
        public string IntershipId { get; set; }

        public Intership Intership { get; set; }

        [Required(ErrorMessage = "You must choose a branch")]
        public Branch Branch { get; set; }

        [Required(ErrorMessage = "You must specify your grades")]
        public string Grades { get; set; }

        [Display(Name = "Unfinished grades")]
        [Required(ErrorMessage = "You must specify your unfinished grades")]
        [DataType(DataType.MultilineText)]
        public string UnfinishedGrades { get; set; }

        public State Result { get; set; }

        public IntershipCandidature()
        {
            Result = State.STANDBY;
        }
    }
}