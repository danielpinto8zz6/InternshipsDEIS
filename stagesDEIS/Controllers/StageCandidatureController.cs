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
    public class StageCandidatureController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StageCandidatureController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StageCandidature
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.StageCandidature.Include(s => s.Candidate).Include(s => s.Stage);

            if (User.IsInRole("Student"))
            {
                return View(await applicationDbContext.Where(s => s.Candidate.Id.Equals(GetUserId())).ToListAsync());
            }
            else if (User.IsInRole("Company"))
            {
                return View(await applicationDbContext.Where(s => s.Stage.CompanyId.Equals(GetUserId())).ToListAsync());
            }
            else if (User.IsInRole("Professor"))
            {
                return View(await applicationDbContext.Where(s => s.Stage.Advisor.Equals(GetUserId())).ToListAsync());
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

        // GET: StageCandidature/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stageCandidature = await _context.StageCandidature
                .Include(s => s.Candidate)
                .Include(s => s.Stage)
                .FirstOrDefaultAsync(m => m.StageCandidatureId == id);
            if (stageCandidature == null)
            {
                return NotFound();
            }

            return View(stageCandidature);
        }

        // GET: StageCandidature/Create
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Create(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stage = await _context.Stage.FindAsync(id);
            if (stage == null)
            {
                return NotFound();
            }

            return View();
        }

        // POST: StageCandidature/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string id, [Bind("StageCandidatureId,StudentId,StageId,Branch,Grades,UnfinishedGrades,Result")] StageCandidature stageCandidature)
        {
            if (ModelState.IsValid)
            {
                stageCandidature.CandidateId = GetUserId();
                stageCandidature.StageId = id;
                _context.Add(stageCandidature);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stageCandidature);
        }

        // GET: StageCandidature/Edit/5
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stageCandidature = await _context.StageCandidature.FindAsync(id);
            if (stageCandidature == null)
            {
                return NotFound();
            }
            return View(stageCandidature);
        }

        // POST: StageCandidature/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("StageCandidatureId,StudentId,StageId,Branch,Grades,UnfinishedGrades,Result")] StageCandidature stageCandidature)
        {
            if (id != stageCandidature.StageCandidatureId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stageCandidature);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StageCandidatureExists(stageCandidature.StageCandidatureId))
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
            return View(stageCandidature);
        }

        // GET: StageCandidature/Delete/5
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stageCandidature = await _context.StageCandidature
                .Include(s => s.Candidate)
                .Include(s => s.Stage)
                .FirstOrDefaultAsync(m => m.StageCandidatureId == id);
            if (stageCandidature == null)
            {
                return NotFound();
            }

            return View(stageCandidature);
        }

        // POST: StageCandidature/Delete/5
        [Authorize(Roles = "Student")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var stageCandidature = await _context.StageCandidature.FindAsync(id);
            _context.StageCandidature.Remove(stageCandidature);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StageCandidatureExists(string id)
        {
            return _context.StageCandidature.Any(e => e.StageCandidatureId == id);
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
