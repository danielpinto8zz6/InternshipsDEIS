using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using InternshipsDEIS.Models;

namespace InternshipsDEIS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<InternshipsDEIS.Models.Project> Project { get; set; }

        public DbSet<InternshipsDEIS.Models.Internship> Internship { get; set; }

        public DbSet<InternshipsDEIS.Models.InternshipCandidature> InternshipCandidature { get; set; }

        public DbSet<InternshipsDEIS.Models.ProjectCandidature> ProjectCandidature { get; set; }

        public DbSet<InternshipsDEIS.Models.Message> Message { get; set; }

        public DbSet<InternshipsDEIS.Models.EvaluateCompany> EvaluateCompany { get; set; }

        public DbSet<InternshipsDEIS.Models.EvaluateStudent> EvaluateStudent { get; set; }
    }
}
