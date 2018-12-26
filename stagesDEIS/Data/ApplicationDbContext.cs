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
        public DbSet<Models.Proposal> Proposal { get; set; }

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

        public DbSet<stagesDEIS.Models.Candidature> Candidature { get; set; }
    }
}
