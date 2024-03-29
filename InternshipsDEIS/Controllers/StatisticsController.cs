using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InternshipsDEIS.Data;
using InternshipsDEIS.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace InternshipsDEIS.Controllers
{
    [Authorize(Roles = "Administrator,Committee")]
    public class StatisticsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EvaluateCompany
        public IActionResult Index()
        {
            var statistics = new Statistics();

            statistics.InternshipsCount = _context.Internship.Count();
            statistics.ProjectsCount = _context.Project.Count();
            statistics.AcceptedInternshipsCount = _context.Internship.Where(i => i.State.Equals(State.ACCEPTED)).Count();
            statistics.AcceptedProjectsCount = _context.Project.Where(i => i.State.Equals(State.ACCEPTED)).Count();
            statistics.InternshipsCandidaturesCount = _context.InternshipCandidature.Count();
            statistics.ProjectsCandidaturesCount = _context.ProjectCandidature.Count();
            statistics.StudentsCount = _context.Users.Where(u => u.Role.Equals("Student")).Count();
            statistics.ProfessorsCount = _context.Users.Where(u => u.Role.Equals("Professor") || u.Role.Equals("Committee")).Count();
            statistics.CompaniesCount = _context.Users.Where(u => u.Role.Equals("Company")).Count();
            statistics.CommitteeMembersCount = _context.Users.Where(u => u.Role.Equals("Committee")).Count();

            return View(statistics);
        }
    }
}