using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternshipsDEIS.Models
{
    public class Project
    {
        public string ProjectId { get; set; }

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

        [Required]
        public Branch Branch { get; set; }

        [Required(ErrorMessage = "You must provide objectives")]
        [DataType(DataType.MultilineText)]
        public string Objectives { get; set; }

        public IList<ApplicationUser> Professors { get; set; }
        public IList<ProjectCandidature> Candidatures { get; set; }

        [DataType(DataType.MultilineText)]
        public string Justification { get; set; }

        public IList<ApplicationUser> Placed { get; set; }

        [ForeignKey("Professor")]
        public string ProfessorId { get; set; }
        
        public ApplicationUser Professor { get; set; }

        public Project()
        {
            State = State.STANDBY;
            Professors = new List<ApplicationUser>();
            Candidatures = new List<ProjectCandidature>();
            Placed = new List<ApplicationUser>();
        }
    }
}