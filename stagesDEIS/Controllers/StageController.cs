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
    public class StageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StageController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Stage
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Stage.Include(s => s.Advisor).Include(s => s.Company);

            // TODO: check if professor have rights to accept/reject
            if (User.IsInRole("Professor"))
            {
                return View(await applicationDbContext.ToListAsync());
            }

            // Show only accepted projects to geral/students...
            return View(await applicationDbContext.Where(s => s.State.Equals(State.ACCEPTED)).ToListAsync());
        }

        // GET: Stage/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stage = await _context.Stage
                .Include(s => s.Advisor)
                .Include(s => s.Company)
                .FirstOrDefaultAsync(m => m.StageId == id);
            if (stage == null)
            {
                return NotFound();
            }

            return View(stage);
        }

        // GET: Stage/Create
        [Authorize(Roles = "Professor,Company")]
        public IActionResult Create()
        {
            ViewData["AdvisorId"] = new SelectList(_context.Professor, "ProfessorId", "ProfessorId");
            ViewData["CompanyId"] = new SelectList(_context.Company, "CompanyId", "CompanyId");
            return View();
        }

        // POST: Stage/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Professor,Company")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StageId,Title,Description,Date,State,AccessConditions,Location,Branch,Objectives,AdvisorId,CompanyId")] Stage stage)
        {
            if (ModelState.IsValid)
            {
                var company = await _context.Company.FindAsync(GetUserId());
                if (company == null)
                {
                    return RedirectToAction("Create", "Company");
                }

                _context.Add(stage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AdvisorId"] = new SelectList(_context.Professor, "ProfessorId", "ProfessorId", stage.AdvisorId);
            return View(stage);
        }

        // GET: Stage/Edit/5
        [Authorize(Roles = "Professor,Company")]
        public async Task<IActionResult> Edit(string id)
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
            ViewData["AdvisorId"] = new SelectList(_context.Professor, "ProfessorId", "ProfessorId", stage.AdvisorId);
            ViewData["CompanyId"] = new SelectList(_context.Company, "CompanyId", "CompanyId", stage.CompanyId);
            return View(stage);
        }

        // POST: Stage/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Professor,Company")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("StageId,Title,Description,Date,State,AccessConditions,Location,Branch,Objectives,AdvisorId,CompanyId")] Stage stage)
        {
            if (id != stage.StageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StageExists(stage.StageId))
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
            ViewData["AdvisorId"] = new SelectList(_context.Professor, "ProfessorId", "ProfessorId", stage.AdvisorId);
            ViewData["CompanyId"] = new SelectList(_context.Company, "CompanyId", "CompanyId", stage.CompanyId);
            return View(stage);
        }

        // GET: Stage/Delete/5
        [Authorize(Roles = "Professor,Company")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stage = await _context.Stage
                .Include(s => s.Advisor)
                .Include(s => s.Company)
                .FirstOrDefaultAsync(m => m.StageId == id);
            if (stage == null)
            {
                return NotFound();
            }

            return View(stage);
        }

        // POST: Stage/Delete/5
        [Authorize(Roles = "Professor,Company")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var stage = await _context.Stage.FindAsync(id);
            _context.Stage.Remove(stage);
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

            var stage = await _context.Stage
                .Include(s => s.Advisor)
                .Include(s => s.Company)
                .FirstOrDefaultAsync(m => m.StageId == id);
            if (stage == null)
            {
                return NotFound();
            }

            var candidature = await _context.StageCandidature
                .FirstOrDefaultAsync(m => m.StageId == id && m.Candidate.Id == GetUserId());

            if (candidature != null)
            {
                return RedirectToAction("Details", "StageCandidature", new { id = candidature.StageCandidatureId });
            }

            return RedirectToAction("Create", "StageCandidature", new { id = id });
        }

        private bool StageExists(string id)
        {
            return _context.Stage.Any(e => e.StageId == id);
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
