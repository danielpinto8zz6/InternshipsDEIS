using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntershipsDEIS.Models
{
    public class Statistics
    {
        public string StatisticsId { get; set; }

        [Display(Name = "Projects")]
        public int ProjectsCount { get; set; }

        [Display(Name = "Interships")]
        public int IntershipsCount { get; set; }

        [Display(Name = "Accepted projects")]
        public int AcceptedProjectsCount { get; set; }

        [Display(Name = "Accepted interships")]
        public int AcceptedIntershipsCount { get; set; }

        [Display(Name = "Projects candidatures")]
        public int ProjectsCandidaturesCount { get; set; }

        [Display(Name = "Interships candidatures")]
        public int IntershipsCandidaturesCount { get; set; }

        [Display(Name = "Students")]
        public int StudentsCount { get; set; }

        [Display(Name = "Professors")]
        public int ProfessorsCount { get; set; }

        [Display(Name = "Companies")]
        public int CompaniesCount { get; set; }

        [Display(Name = "Committee Members")]
        public int CommitteeMembersCount { get; set; }

        public IList<ApplicationUser> Companies { get; set; }

        public Statistics()
        {
            Companies = new List<ApplicationUser>();
        }
    }
}