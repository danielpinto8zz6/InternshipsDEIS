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
    public class ProposalController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProposalController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Proposal
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Proposal.Include(p => p.Company).Include(p => p.Placed).Include(p => p.Professor);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Proposal/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proposal = await _context.Proposal
                .Include(p => p.Company)
                .Include(p => p.Placed)
                .Include(p => p.Professor)
                .FirstOrDefaultAsync(m => m.ProposalId == id);
            if (proposal == null)
            {
                return NotFound();
            }

            return View(proposal);
        }

        [Authorize(Roles = "Professor,Company")]
        public IActionResult Create()
        {
            this.ViewData["Companies"] = _context.Company
                .Select(c => new SelectListItem() { Text = c.User.UserName, Value = c.CompanyId })
                .ToList();

            this.ViewData["Placed"] = _context.Student
                .Select(s => new SelectListItem() { Text = s.User.UserName, Value = s.StudentId })
                .ToList();

            this.ViewData["Professor"] = _context.Professor
                .Select(p => new SelectListItem() { Text = p.User.UserName, Value = p.ProfessorId })
                .ToList();

            return View();
        }

        // POST: Proposal/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Professor,Company")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProposalId,Title,Description,State,AccessConditions,Location,Branch,Objectives,ProfessorId,CompanyId,PlacedId,Justification")] Proposal proposal)
        {
            if (ModelState.IsValid)
            {
                DateTime dateTime = DateTime.UtcNow.Date;
                proposal.Date = dateTime;
                _context.Add(proposal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Company, "CompanyId", "CompanyId", proposal.CompanyId);
            ViewData["PlacedId"] = new SelectList(_context.Student, "StudentId", "StudentId", proposal.PlacedId);
            ViewData["ProfessorId"] = new SelectList(_context.Professor, "ProfessorId", "ProfessorId", proposal.ProfessorId);
            return View(proposal);
        }

        [Authorize(Roles = "Professor,Company")]
        public async Task<IActionResult> Edit(string id)
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
            ViewData["CompanyId"] = new SelectList(_context.Company, "CompanyId", "CompanyId", proposal.CompanyId);
            ViewData["PlacedId"] = new SelectList(_context.Student, "StudentId", "StudentId", proposal.PlacedId);
            ViewData["ProfessorId"] = new SelectList(_context.Professor, "ProfessorId", "ProfessorId", proposal.ProfessorId);
            return View(proposal);
        }

        // POST: Proposal/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Professor,Company")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ProposalId,Title,Description,Date,State,AccessConditions,Location,Branch,Objectives,ProfessorId,CompanyId,PlacedId,Justification")] Proposal proposal)
        {
            if (id != proposal.ProposalId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proposal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProposalExists(proposal.ProposalId))
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
            ViewData["CompanyId"] = new SelectList(_context.Company, "CompanyId", "CompanyId", proposal.CompanyId);
            ViewData["PlacedId"] = new SelectList(_context.Student, "StudentId", "StudentId", proposal.PlacedId);
            ViewData["ProfessorId"] = new SelectList(_context.Professor, "ProfessorId", "ProfessorId", proposal.ProfessorId);
            return View(proposal);
        }

        [Authorize(Roles = "Professor,Company")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proposal = await _context.Proposal
                .Include(p => p.Company)
                .Include(p => p.Placed)
                .Include(p => p.Professor)
                .FirstOrDefaultAsync(m => m.ProposalId == id);
            if (proposal == null)
            {
                return NotFound();
            }

            return View(proposal);
        }

        [Authorize(Roles = "Professor,Company")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var proposal = await _context.Proposal.FindAsync(id);
            _context.Proposal.Remove(proposal);
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

            var proposal = await _context.Proposal
                .Include(p => p.Company)
                .Include(p => p.Placed)
                .Include(p => p.Professor)
                .FirstOrDefaultAsync(m => m.ProposalId == id);
            if (proposal == null)
            {
                return NotFound();
            }

            var candidature = await _context.Candidature
                .FirstOrDefaultAsync(m => m.ProposalId == id && m.Candidate.Id == GetUserId());

            if (candidature != null)
            {
                return RedirectToAction("Details", "Candidature", new { id = candidature.CandidatureId });
            }

            return RedirectToAction("Create", "Candidature", new { id = id });
        }

        private bool ProposalExists(string id)
        {
            return _context.Proposal.Any(e => e.ProposalId == id);
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
