using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IntershipsDEIS.Data;
using IntershipsDEIS.Models;

namespace IntershipsDEIS.Controllers
{
    public class IntershipCandidatureController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IntershipCandidatureController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: IntershipCandidature
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.IntershipCandidature.Include(s => s.Candidate).Include(s => s.Intership);

            if (User.IsInRole("Student"))
            {
                return View(await applicationDbContext.Where(s => s.Candidate.Id.Equals(GetUserId())).ToListAsync());
            }
            else if (User.IsInRole("Company"))
            {
                return View(await applicationDbContext.Where(s => s.IntershipId.Equals(GetUserId())).ToListAsync());
            }
            else if (User.IsInRole("Administrator") || User.IsInRole("Committee"))
            {
                return View(await applicationDbContext.ToListAsync());
            }
            else if (User.IsInRole("Professor"))
            {
                return View(await applicationDbContext.Where(s => s.Intership.Advisor.Equals(GetUserId())).ToListAsync());
            }
            else
            {
                return NotFound();
            }
        }

        // GET: IntershipCandidature/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var IntershipCandidature = await _context.IntershipCandidature
                .Include(s => s.Candidate)
                .Include(s => s.Intership)
                .FirstOrDefaultAsync(m => m.IntershipCandidatureId == id);
            if (IntershipCandidature == null)
            {
                return NotFound();
            }

            return View(IntershipCandidature);
        }

        // GET: IntershipCandidature/Create
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Create(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Intership = await _context.Intership.FindAsync(id);
            if (Intership == null)
            {
                return NotFound();
            }

            return View();
        }

        // POST: IntershipCandidature/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string id, [Bind("IntershipCandidatureId,StudentId,IntershipId,Branch,Grades,UnfinishedGrades,Result")] IntershipCandidature IntershipCandidature)
        {
            if (ModelState.IsValid)
            {
                IntershipCandidature.CandidateId = GetUserId();
                IntershipCandidature.IntershipId = id;
                _context.Add(IntershipCandidature);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(IntershipCandidature);
        }

        // GET: IntershipCandidature/Edit/5
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var IntershipCandidature = await _context.IntershipCandidature.FindAsync(id);
            if (IntershipCandidature == null)
            {
                return NotFound();
            }
            return View(IntershipCandidature);
        }

        // POST: IntershipCandidature/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IntershipCandidatureId,StudentId,IntershipId,Branch,Grades,UnfinishedGrades,Result")] IntershipCandidature IntershipCandidature)
        {
            if (id != IntershipCandidature.IntershipCandidatureId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(IntershipCandidature);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IntershipCandidatureExists(IntershipCandidature.IntershipCandidatureId))
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
            return View(IntershipCandidature);
        }

        // GET: IntershipCandidature/Delete/5
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var IntershipCandidature = await _context.IntershipCandidature
                .Include(s => s.Candidate)
                .Include(s => s.Intership)
                .FirstOrDefaultAsync(m => m.IntershipCandidatureId == id);
            if (IntershipCandidature == null)
            {
                return NotFound();
            }

            return View(IntershipCandidature);
        }

        // POST: IntershipCandidature/Delete/5
        [Authorize(Roles = "Student")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var IntershipCandidature = await _context.IntershipCandidature.FindAsync(id);
            _context.IntershipCandidature.Remove(IntershipCandidature);
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

            var intershipCandidature = await _context.IntershipCandidature
                .Include(s => s.Candidate)
                .Include(s => s.Intership)
                .FirstOrDefaultAsync(m => m.IntershipCandidatureId == id);
            if (intershipCandidature == null)
            {
                return NotFound();
            }

            // Only company can accept
            if (!intershipCandidature.Intership.CompanyId.Equals(GetUserId()))
            {
                return NotFound();
            }

            intershipCandidature.Result = State.ACCEPTED;

            var intership = intershipCandidature.Intership;
            intership.Placed.Add(intershipCandidature.Candidate);

            _context.Update(intershipCandidature);
            _context.Update(intership);
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

            var intershipCandidature = await _context.IntershipCandidature
                .Include(s => s.Candidate)
                .Include(s => s.Intership)
                .FirstOrDefaultAsync(m => m.IntershipCandidatureId == id);
            if (intershipCandidature == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Committee"))
            {
                // Only company or committee can reject
                if (!intershipCandidature.Intership.CompanyId.Equals(GetUserId()))
                {
                    return NotFound();
                }
            }

            intershipCandidature.Result = State.REJECTED;

            _context.Update(intershipCandidature);
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

            var intershipCandidature = await _context.IntershipCandidature
                .Include(s => s.Candidate)
                .Include(s => s.Intership)
                .FirstOrDefaultAsync(m => m.IntershipCandidatureId == id);
            if (intershipCandidature == null)
            {
                return NotFound();
            }

            return RedirectToAction("Create", "EvaluateCompany", new { id = intershipCandidature.IntershipId });
        }

        private bool IntershipCandidatureExists(string id)
        {
            return _context.IntershipCandidature.Any(e => e.IntershipCandidatureId == id);
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
