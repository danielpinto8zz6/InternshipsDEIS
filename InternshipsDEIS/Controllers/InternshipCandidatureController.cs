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
    public class InternshipCandidatureController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InternshipCandidatureController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: InternshipCandidature
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.InternshipCandidature.Include(s => s.Candidate).Include(s => s.Internship);

            if (User.IsInRole("Student"))
            {
                return View(await applicationDbContext.Where(s => s.Candidate.Id.Equals(GetUserId())).ToListAsync());
            }
            else if (User.IsInRole("Company"))
            {
                return View(await applicationDbContext.Where(s => s.InternshipId.Equals(GetUserId())).ToListAsync());
            }
            else if (User.IsInRole("Administrator") || User.IsInRole("Committee"))
            {
                return View(await applicationDbContext.ToListAsync());
            }
            else if (User.IsInRole("Professor"))
            {
                return View(await applicationDbContext.Where(s => s.Internship.Advisor.Equals(GetUserId())).ToListAsync());
            }
            else
            {
                return NotFound();
            }
        }

        // GET: InternshipCandidature/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var InternshipCandidature = await _context.InternshipCandidature
                .Include(s => s.Candidate)
                .Include(s => s.Internship)
                .FirstOrDefaultAsync(m => m.InternshipCandidatureId == id);
            if (InternshipCandidature == null)
            {
                return NotFound();
            }

            return View(InternshipCandidature);
        }

        // GET: InternshipCandidature/Create
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Create(string id)
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

            return View();
        }

        // POST: InternshipCandidature/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string id, [Bind("InternshipCandidatureId,StudentId,InternshipId,Branch,Grades,UnfinishedGrades,Result")] InternshipCandidature InternshipCandidature)
        {
            if (ModelState.IsValid)
            {
                InternshipCandidature.CandidateId = GetUserId();
                InternshipCandidature.InternshipId = id;
                _context.Add(InternshipCandidature);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(InternshipCandidature);
        }

        // GET: InternshipCandidature/Edit/5
        [Authorize(Roles = "Administrator,Student")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var InternshipCandidature = await _context.InternshipCandidature.FindAsync(id);
            if (InternshipCandidature == null)
            {
                return NotFound();
            }
            return View(InternshipCandidature);
        }

        // POST: InternshipCandidature/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator,Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("InternshipCandidatureId,StudentId,InternshipId,Branch,Grades,UnfinishedGrades,Result")] InternshipCandidature InternshipCandidature)
        {
            if (id != InternshipCandidature.InternshipCandidatureId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(InternshipCandidature);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InternshipCandidatureExists(InternshipCandidature.InternshipCandidatureId))
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
            return View(InternshipCandidature);
        }

        // GET: InternshipCandidature/Delete/5
        [Authorize(Roles = "Administrator,Student")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var InternshipCandidature = await _context.InternshipCandidature
                .Include(s => s.Candidate)
                .Include(s => s.Internship)
                .FirstOrDefaultAsync(m => m.InternshipCandidatureId == id);
            if (InternshipCandidature == null)
            {
                return NotFound();
            }

            return View(InternshipCandidature);
        }

        // POST: InternshipCandidature/Delete/5
        [Authorize(Roles = "Administrator,Student")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var InternshipCandidature = await _context.InternshipCandidature.FindAsync(id);
            _context.InternshipCandidature.Remove(InternshipCandidature);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Company")]
        public async Task<IActionResult> Accept(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var internshipCandidature = await _context.InternshipCandidature
                .Include(s => s.Candidate)
                .Include(s => s.Internship)
                .FirstOrDefaultAsync(m => m.InternshipCandidatureId == id);
            if (internshipCandidature == null)
            {
                return NotFound();
            }

            // Only company can accept
            if (!internshipCandidature.Internship.CompanyId.Equals(GetUserId()))
            {
                return NotFound();
            }

            internshipCandidature.Result = State.ACCEPTED;

            var internship = internshipCandidature.Internship;
            internship.Placed.Add(internshipCandidature.Candidate);

            _context.Update(internshipCandidature);
            _context.Update(internship);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Professor,Committee")]
        public async Task<IActionResult> Reject(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var internshipCandidature = await _context.InternshipCandidature
                .Include(s => s.Candidate)
                .Include(s => s.Internship)
                .FirstOrDefaultAsync(m => m.InternshipCandidatureId == id);
            if (internshipCandidature == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Committee"))
            {
                // Only company or committee can reject
                if (!internshipCandidature.Internship.CompanyId.Equals(GetUserId()))
                {
                    return NotFound();
                }
            }

            internshipCandidature.Result = State.REJECTED;

            _context.Update(internshipCandidature);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Evaluate(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var internshipCandidature = await _context.InternshipCandidature
                .Include(s => s.Candidate)
                .Include(s => s.Internship)
                .FirstOrDefaultAsync(m => m.InternshipCandidatureId == id);
            if (internshipCandidature == null)
            {
                return NotFound();
            }

            return RedirectToAction("Create", "EvaluateCompany", new { id = internshipCandidature.InternshipId });
        }

        private bool InternshipCandidatureExists(string id)
        {
            return _context.InternshipCandidature.Any(e => e.InternshipCandidatureId == id);
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
