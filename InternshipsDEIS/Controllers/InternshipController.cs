using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InternshipsDEIS.Data;
using InternshipsDEIS.Models;

namespace InternshipsDEIS.Controllers
{
    public class InternshipController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InternshipController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Internship
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Internship.Include(s => s.Advisor).Include(s => s.Company);

            // TODO: check if professor have rights to accept/reject
            if (User.IsInRole("Committee") || User.IsInRole("Administrator"))
            {
                return View(await applicationDbContext.ToListAsync());
            }

            // Show only accepted projects to geral/students...
            return View(await applicationDbContext.Where(s => s.State.Equals(State.ACCEPTED)).ToListAsync());
        }

        [Authorize(Roles = "Company")]
        public async Task<IActionResult> Mine()
        {
            var applicationDbContext = _context.Internship.Include(s => s.Advisor).Include(s => s.Company);
            return View(await applicationDbContext.Where(c => c.CompanyId.Equals(GetUserId())).ToListAsync());
        }

        // GET: Internship/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Internship = await _context.Internship
                .Include(s => s.Advisor)
                .Include(s => s.Company)
                .FirstOrDefaultAsync(m => m.InternshipId == id);
            if (Internship == null)
            {
                return NotFound();
            }

            return View(Internship);
        }

        // GET: Internship/Create
        [Authorize(Roles = "Company")]
        public IActionResult Create()
        {
            ViewData["AdvisorId"] = new SelectList(_context.Users.Where(c => c.Role.Equals("Professor") || c.Role.Equals("Committee")).ToList(), "Id", "UserName");
            return View();
        }

        // POST: Internship/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Company")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InternshipId,Title,Description,Date,State,AccessConditions,Location,Branch,Objectives,AdvisorId,CompanyId")] Internship Internship)
        {
            if (ModelState.IsValid)
            {
                Internship.CompanyId = GetUserId();
                Internship.Date = DateTime.UtcNow;
                _context.Add(Internship);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["AdvisorId"] = new SelectList(_context.Users.Where(c => c.Role.Equals("Professor") || c.Role.Equals("Committee")).ToList(), "Id", "UserName", Internship.AdvisorId);

            return View(Internship);
        }

        // GET: Internship/Edit/5
        [Authorize(Roles = "Administrator,Company")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Internship = await _context.Internship.FindAsync(id);
            if (Internship == null)
            {
                return NotFound();
            }
            ViewData["AdvisorId"] = new SelectList(_context.Users.Where(c => c.Role.Equals("Professor") || c.Role.Equals("Committee")).ToList(), "Id", "UserName", Internship.AdvisorId);
            ViewData["CompanyId"] = new SelectList(_context.Users.Where(c => c.Role.Equals("Company")).ToList(), "Id", "UserName", Internship.CompanyId);
            return View(Internship);
        }

        // POST: Internship/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator,Company")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("InternshipId,Title,Description,Date,State,AccessConditions,Location,Branch,Objectives,AdvisorId,CompanyId")] Internship Internship)
        {
            if (id != Internship.InternshipId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Internship);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InternshipExists(Internship.InternshipId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AdvisorId"] = new SelectList(_context.Users.Where(c => c.Role.Equals("Professor") || c.Role.Equals("Committee")).ToList(), "Id", "UserName", Internship.AdvisorId);
            ViewData["CompanyId"] = new SelectList(_context.Users.Where(c => c.Role.Equals("Company")).ToList(), "Id", "UserName", Internship.CompanyId);
            return View(Internship);
        }

        // GET: Internship/Delete/5
        [Authorize(Roles = "Administrator,Company")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Internship = await _context.Internship
                .Include(s => s.Advisor)
                .Include(s => s.Company)
                .FirstOrDefaultAsync(m => m.InternshipId == id);
            if (Internship == null)
            {
                return NotFound();
            }

            return View(Internship);
        }

        // POST: Internship/Delete/5
        [Authorize(Roles = "Administrator,Company")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var Internship = await _context.Internship.FindAsync(id);
            _context.Internship.Remove(Internship);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Apply(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Internship = await _context.Internship
                .Include(s => s.Advisor)
                .Include(s => s.Company)
                .FirstOrDefaultAsync(m => m.InternshipId == id);
            if (Internship == null)
            {
                return NotFound();
            }

            var candidature = await _context.InternshipCandidature
                .FirstOrDefaultAsync(m => m.InternshipId == id && m.Candidate.Id == GetUserId());

            if (candidature != null)
            {
                return RedirectToAction("Details", "InternshipCandidature", new { id = candidature.InternshipCandidatureId });
            }

            return RedirectToAction("Create", "InternshipCandidature", new { id = id });
        }

        [Authorize(Roles = "Committee")]
        public async Task<IActionResult> Accept(string id)
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

            // Accept
            internship.State = State.ACCEPTED;

            _context.Update(internship);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Committee")]
        public async Task<IActionResult> Reject(string id)
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

            // Accept
            internship.State = State.REJECTED;

            _context.Update(internship);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool InternshipExists(string id)
        {
            return _context.Internship.Any(e => e.InternshipId == id);
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
