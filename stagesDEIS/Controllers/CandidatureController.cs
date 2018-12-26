using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using stagesDEIS.Data;
using stagesDEIS.Models;

namespace stagesDEIS.Controllers
{
    public class CandidatureController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CandidatureController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Candidature
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Candidature.Include(c => c.Proposal).Include(c => c.Candidate);
            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize(Roles = "Professor,Company")]
        public async Task<IActionResult> Accept(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidature = await _context.Candidature
                .Include(c => c.Proposal)
                .Include(c => c.Candidate)
                .FirstOrDefaultAsync(m => m.CandidatureId == id);
            if (candidature == null)
            {
                return NotFound();
            }

            candidature.Result = State.ACCEPTED;

            _context.Update(candidature);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Professor,Company")]
        public async Task<IActionResult> Reject(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidature = await _context.Candidature
                .Include(c => c.Proposal)
                .Include(c => c.Candidate)
                .FirstOrDefaultAsync(m => m.CandidatureId == id);
            if (candidature == null)
            {
                return NotFound();
            }

            candidature.Result = State.REJECTED;

            _context.Update(candidature);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidature = await _context.Candidature
                .Include(c => c.Proposal)
                .Include(c => c.Candidate)
                .FirstOrDefaultAsync(m => m.CandidatureId == id);
            if (candidature == null)
            {
                return NotFound();
            }

            return View(candidature);
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Create(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proposal = await _context.Proposal.FindAsync(id);

            if (proposal == null)
            {
                return NotFound();
            }

            return View();
        }

        // POST: Candidature/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string id, [Bind("CandidatureId,Branch,Grades,UnfinishedGrades")] Candidature candidature)
        {
            if (ModelState.IsValid)
            {
                candidature.StudentId = GetUserId();
                candidature.ProposalId = id;
                _context.Add(candidature);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(candidature);
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidature = await _context.Candidature.FindAsync(id);
            if (candidature == null)
            {
                return NotFound();
            }

            return View(candidature);
        }

        // POST: Candidature/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CandidatureId,Branch,Grades,UnfinishedGrades")] Candidature candidature)
        {
            if (id != candidature.CandidatureId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(candidature);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CandidatureExists(candidature.CandidatureId))
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
            return View(candidature);
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidature = await _context.Candidature
                .Include(c => c.Proposal)
                .Include(c => c.Candidate)
                .FirstOrDefaultAsync(m => m.CandidatureId == id);
            if (candidature == null)
            {
                return NotFound();
            }

            return View(candidature);
        }

        [Authorize(Roles = "Student")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var candidature = await _context.Candidature.FindAsync(id);
            _context.Candidature.Remove(candidature);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CandidatureExists(string id)
        {
            return _context.Candidature.Any(e => e.CandidatureId == id);
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
