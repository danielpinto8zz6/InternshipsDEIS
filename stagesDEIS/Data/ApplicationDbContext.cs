using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using stagesDEIS.Models;

namespace stagesDEIS.Data
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

        public DbSet<stagesDEIS.Models.Company> Company { get; set; }

        public DbSet<stagesDEIS.Models.Student> Student { get; set; }

        public DbSet<stagesDEIS.Models.Professor> Professor { get; set; }

        public DbSet<stagesDEIS.Models.Project> Project { get; set; }

        public DbSet<stagesDEIS.Models.Stage> Stage { get; set; }

        public DbSet<stagesDEIS.Models.StageCandidature> StageCandidature { get; set; }

        public DbSet<stagesDEIS.Models.ProjectCandidature> ProjectCandidature { get; set; }
    }
}
