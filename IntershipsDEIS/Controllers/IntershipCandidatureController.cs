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
                return View(await applicationDbContext.Where(s => s.Intership.CompanyId.Equals(GetUserId())).ToListAsync());
            }
            else if (User.IsInRole("Professor"))
            {
                return View(await applicationDbContext.Where(s => s.Intership.Advisor.Equals(GetUserId())).ToListAsync());
            }
            else if (User.IsInRole("Administrator"))
            {
                return View(await applicationDbContext.ToListAsync());
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