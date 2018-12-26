using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stagesDEIS.Models
{
    public class Stage
    {
        public string StageId { get; set; }

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

        [Required(ErrorMessage = "You must specify stage location")]
        [DataType(DataType.MultilineText)]
        public string Location { get; set; }

        [Required]
        public Branch Branch { get; set; }

        [Required(ErrorMessage = "You must provide objectives")]
        [DataType(DataType.MultilineText)]
        public string Objectives { get; set; }

        public Professor Advisor { get; set; }

        [ForeignKey("Professor")]
        public string AdvisorId { get; set; }

        public IList<StageCandidature> Candidatures { get; set; }

        public Company Company { get; set; }

        [ForeignKey("Company")]
        public string CompanyId { get; set; }

        [DataType(DataType.MultilineText)]
        public string Justification { get; set; }

        public Stage()
        {
            State = State.STANDBY;
            Candidatures = new List<StageCandidature>();
        }
    }
}