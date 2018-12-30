﻿// <auto-generated />
using System;
using InternshipsDEIS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IntershipsDEIS.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20181230013709_CreateDatabase")]
    partial class CreateDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity("InternshipsDEIS.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("InternshipId");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("ProjectId");

                    b.Property<string>("ProjectId1");

                    b.Property<string>("Role");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("InternshipId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.HasIndex("ProjectId");

                    b.HasIndex("ProjectId1");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("InternshipsDEIS.Models.EvaluateCompany", b =>
                {
                    b.Property<string>("EvaluateCompanyId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompanyId");

                    b.Property<string>("Justification")
                        .IsRequired();

                    b.Property<int>("Pontuation");

                    b.Property<string>("StudentId");

                    b.HasKey("EvaluateCompanyId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("StudentId");

                    b.ToTable("EvaluateCompany");
                });

            modelBuilder.Entity("InternshipsDEIS.Models.EvaluateStudent", b =>
                {
                    b.Property<string>("EvaluateStudentId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EntityId");

                    b.Property<string>("Justification")
                        .IsRequired();

                    b.Property<int>("Pontuation");

                    b.Property<string>("StudentId");

                    b.HasKey("EvaluateStudentId");

                    b.HasIndex("EntityId");

                    b.HasIndex("StudentId");

                    b.ToTable("EvaluateStudent");
                });

            modelBuilder.Entity("InternshipsDEIS.Models.Internship", b =>
                {
                    b.Property<string>("InternshipId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccessConditions")
                        .IsRequired();

                    b.Property<string>("AdvisorId");

                    b.Property<int>("Branch");

                    b.Property<string>("CompanyId");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("Justification");

                    b.Property<string>("Location")
                        .IsRequired();

                    b.Property<string>("Objectives")
                        .IsRequired();

                    b.Property<int>("State");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("InternshipId");

                    b.HasIndex("AdvisorId");

                    b.HasIndex("CompanyId");

                    b.ToTable("Internship");
                });

            modelBuilder.Entity("InternshipsDEIS.Models.InternshipCandidature", b =>
                {
                    b.Property<string>("InternshipCandidatureId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Branch");

                    b.Property<string>("CandidateId");

                    b.Property<DateTime>("DefenseDate");

                    b.Property<string>("Grades")
                        .IsRequired();

                    b.Property<string>("InternshipId");

                    b.Property<int>("Result");

                    b.Property<string>("UnfinishedGrades")
                        .IsRequired();

                    b.HasKey("InternshipCandidatureId");

                    b.HasIndex("CandidateId");

                    b.HasIndex("InternshipId");

                    b.ToTable("InternshipCandidature");
                });

            modelBuilder.Entity("InternshipsDEIS.Models.Message", b =>
                {
                    b.Property<string>("MessageId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<string>("RecipientId")
                        .IsRequired();

                    b.Property<string>("SenderId");

                    b.Property<string>("Text")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<bool>("read");

                    b.HasKey("MessageId");

                    b.HasIndex("RecipientId");

                    b.HasIndex("SenderId");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("InternshipsDEIS.Models.Project", b =>
                {
                    b.Property<string>("ProjectId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccessConditions")
                        .IsRequired();

                    b.Property<int>("Branch");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("Justification");

                    b.Property<string>("Objectives")
                        .IsRequired();

                    b.Property<int>("State");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("ProjectId");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("InternshipsDEIS.Models.ProjectCandidature", b =>
                {
                    b.Property<string>("ProjectCandidatureId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Branch");

                    b.Property<string>("CandidateId");

                    b.Property<DateTime>("DefenseDate");

                    b.Property<string>("Grades")
                        .IsRequired();

                    b.Property<string>("ProjectId");

                    b.Property<int>("Result");

                    b.Property<string>("UnfinishedGrades")
                        .IsRequired();

                    b.HasKey("ProjectCandidatureId");

                    b.HasIndex("CandidateId");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectCandidature");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("InternshipsDEIS.Models.ApplicationUser", b =>
                {
                    b.HasOne("InternshipsDEIS.Models.Internship")
                        .WithMany("Placed")
                        .HasForeignKey("InternshipId");

                    b.HasOne("InternshipsDEIS.Models.Project")
                        .WithMany("Placed")
                        .HasForeignKey("ProjectId");

                    b.HasOne("InternshipsDEIS.Models.Project")
                        .WithMany("Professors")
                        .HasForeignKey("ProjectId1");
                });

            modelBuilder.Entity("InternshipsDEIS.Models.EvaluateCompany", b =>
                {
                    b.HasOne("InternshipsDEIS.Models.ApplicationUser", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId");

                    b.HasOne("InternshipsDEIS.Models.ApplicationUser", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId");
                });

            modelBuilder.Entity("InternshipsDEIS.Models.EvaluateStudent", b =>
                {
                    b.HasOne("InternshipsDEIS.Models.ApplicationUser", "Entity")
                        .WithMany()
                        .HasForeignKey("EntityId");

                    b.HasOne("InternshipsDEIS.Models.ApplicationUser", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId");
                });

            modelBuilder.Entity("InternshipsDEIS.Models.Internship", b =>
                {
                    b.HasOne("InternshipsDEIS.Models.ApplicationUser", "Advisor")
                        .WithMany()
                        .HasForeignKey("AdvisorId");

                    b.HasOne("InternshipsDEIS.Models.ApplicationUser", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId");
                });

            modelBuilder.Entity("InternshipsDEIS.Models.InternshipCandidature", b =>
                {
                    b.HasOne("InternshipsDEIS.Models.ApplicationUser", "Candidate")
                        .WithMany()
                        .HasForeignKey("CandidateId");

                    b.HasOne("InternshipsDEIS.Models.Internship", "Internship")
                        .WithMany("Candidatures")
                        .HasForeignKey("InternshipId");
                });

            modelBuilder.Entity("InternshipsDEIS.Models.Message", b =>
                {
                    b.HasOne("InternshipsDEIS.Models.ApplicationUser", "Recipient")
                        .WithMany()
                        .HasForeignKey("RecipientId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("InternshipsDEIS.Models.ApplicationUser", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId");
                });

            modelBuilder.Entity("InternshipsDEIS.Models.ProjectCandidature", b =>
                {
                    b.HasOne("InternshipsDEIS.Models.ApplicationUser", "Candidate")
                        .WithMany()
                        .HasForeignKey("CandidateId");

                    b.HasOne("InternshipsDEIS.Models.Project", "Project")
                        .WithMany("Candidatures")
                        .HasForeignKey("ProjectId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("InternshipsDEIS.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("InternshipsDEIS.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("InternshipsDEIS.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("InternshipsDEIS.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}