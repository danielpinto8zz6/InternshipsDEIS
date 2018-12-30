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
    [Authorize(Roles = "Student,Committee")]
    public class EvaluateCompanyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EvaluateCompanyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EvaluateCompany
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.EvaluateCompany.Include(e => e.Company).Include(e => e.Student);

            if (User.IsInRole("Committee"))
            {
                return View(await applicationDbContext.ToListAsync());
            }

            return View(await applicationDbContext.Where(e => e.StudentId.Equals(GetUserId())).ToListAsync());
        }

        // GET: EvaluateCompany/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evaluateCompany = await _context.EvaluateCompany
                .Include(e => e.Company)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.EvaluateCompanyId == id);
            if (evaluateCompany == null)
            {
                return NotFound();
            }

            return View(evaluateCompany);
        }

        // GET: EvaluateCompany/Create
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Create(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var internship = await _context.Internship.FindAsync(id);
            if (internship == null)
            {
                return NotFound();
            }

            // You can't evaluate if you wasn't placed 
            var student = await _context.Users.FindAsync(GetUserId());
            if (!internship.Placed.Contains(student))
            {
                return NotFound();
            }

            return View();
        }

        // POST: EvaluateCompany/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string id, [Bind("EvaluateCompanyId,CompanyId,Pontuation,Justification")] EvaluateCompany evaluateCompany)
        {
            if (ModelState.IsValid)
            {
                var internship = await _context.Internship.FindAsync(id);
                if (internship == null)
                {
                    return NotFound();
                }

                evaluateCompany.CompanyId = internship.CompanyId;
                evaluateCompany.StudentId = GetUserId();
                _context.Add(evaluateCompany);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Users, "Id", "Id", evaluateCompany.CompanyId);
            return View(evaluateCompany);
        }

        private bool EvaluateCompanyExists(string id)
        {
            return _context.EvaluateCompany.Any(e => e.EvaluateCompanyId == id);
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
