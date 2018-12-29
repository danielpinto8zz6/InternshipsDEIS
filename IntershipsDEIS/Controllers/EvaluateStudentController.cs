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
    [Authorize(Roles = "Company,Professor,Committee")]
    public class EvaluateStudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EvaluateStudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EvaluateStudent
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.EvaluateStudent.Include(e => e.Entity).Include(e => e.Student);

            if (User.IsInRole("Committee"))
            {
                return View(await applicationDbContext.ToListAsync());
            }
            
            return View(await applicationDbContext.Where(e => e.EntityId.Equals(GetUserId())).ToListAsync());
        }

        // GET: EvaluateStudent/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evaluateStudent = await _context.EvaluateStudent
                .Include(e => e.Entity)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.EvaluateStudentId == id);
            if (evaluateStudent == null)
            {
                return NotFound();
            }

            return View(evaluateStudent);
        }

        // GET: EvaluateStudent/Create
        [Authorize(Roles = "Company,Professor")]
        public IActionResult Create()
        {
            ViewData["StudentId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: EvaluateStudent/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Company,Professor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EvaluateStudentId,StudentId,Pontuation,Justification")] EvaluateStudent evaluateStudent)
        {
            if (ModelState.IsValid)
            {
                evaluateStudent.EntityId = GetUserId();
                _context.Add(evaluateStudent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentId"] = new SelectList(_context.Users, "Id", "Id", evaluateStudent.StudentId);
            return View(evaluateStudent);
        }

        private bool EvaluateStudentExists(string id)
        {
            return _context.EvaluateStudent.Any(e => e.EvaluateStudentId == id);
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
