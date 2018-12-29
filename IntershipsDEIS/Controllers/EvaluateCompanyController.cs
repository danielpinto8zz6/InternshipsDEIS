using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IntershipsDEIS.Data;
using IntershipsDEIS.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace IntershipsDEIS.Controllers
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

            if (User.IsInRole("Student"))
            {
                return View(await applicationDbContext.Where(e => e.StudentId.Equals(GetUserId())).ToListAsync());
            }

            return View(await applicationDbContext.ToListAsync());
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

            var intership = await _context.Intership.FindAsync(id);
            if (intership == null)
            {
                return NotFound();
            }

            // You can't evaluate if you wasn't placed 
            var student = await _context.Users.FindAsync(GetUserId());
            if (!intership.Placed.Contains(student))
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
                var intership = await _context.Intership.FindAsync(id);
                if (intership == null)
                {
                    return NotFound();
                }

                evaluateCompany.CompanyId = intership.CompanyId;
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
