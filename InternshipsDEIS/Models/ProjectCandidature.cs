using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace InternshipsDEIS.Models
{
    public class ProjectCandidature
    {
        public string ProjectCandidatureId { get; set; }

        [ForeignKey("Candidate")]
        public string CandidateId { get; set; }

        public ApplicationUser Candidate { get; set; }

        [ForeignKey("Project")]
        public string ProjectId { get; set; }

        public Project Project { get; set; }

        [Required(ErrorMessage = "You must choose a branch")]
        public Branch Branch { get; set; }

        [Required(ErrorMessage = "You must specify your grades")]
        public string Grades { get; set; }

        [Display(Name = "Unfinished grades")]
        [Required(ErrorMessage = "You must specify your unfinished grades")]
        [DataType(DataType.MultilineText)]
        public string UnfinishedGrades { get; set; }

        public State Result { get; set; }

        public DateTime DefenseDate { get; set; }

        public ProjectCandidature()
        {
            Result = State.STANDBY;
        }
    }
}