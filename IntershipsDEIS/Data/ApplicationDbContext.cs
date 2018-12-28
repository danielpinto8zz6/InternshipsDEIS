using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IntershipsDEIS.Models;

namespace IntershipsDEIS.Data
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

        public DbSet<IntershipsDEIS.Models.Project> Project { get; set; }

        public DbSet<IntershipsDEIS.Models.Intership> Intership { get; set; }

        public DbSet<IntershipsDEIS.Models.IntershipCandidature> IntershipCandidature { get; set; }

        public DbSet<IntershipsDEIS.Models.ProjectCandidature> ProjectCandidature { get; set; }

        public DbSet<IntershipsDEIS.Models.Message> Message { get; set; }

        public DbSet<IntershipsDEIS.Models.EvaluateCompany> EvaluateCompany { get; set; }

        public DbSet<IntershipsDEIS.Models.EvaluateStudent> EvaluateStudent { get; set; }
    }
}
