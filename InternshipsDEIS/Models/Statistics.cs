using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InternshipsDEIS.Models
{
    public class Statistics
    {
        public string StatisticsId { get; set; }

        [Display(Name = "Projects")]
        public int ProjectsCount { get; set; }

        [Display(Name = "Internships")]
        public int InternshipsCount { get; set; }

        [Display(Name = "Accepted projects")]
        public int AcceptedProjectsCount { get; set; }

        [Display(Name = "Accepted internships")]
        public int AcceptedInternshipsCount { get; set; }

        [Display(Name = "Projects candidatures")]
        public int ProjectsCandidaturesCount { get; set; }

        [Display(Name = "Internships candidatures")]
        public int InternshipsCandidaturesCount { get; set; }

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