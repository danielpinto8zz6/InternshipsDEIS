using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Details(int? id)
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

        // GET: Proposal/Create
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Set<Company>(), "CompanyId", "Address");
            ViewData["PlacedId"] = new SelectList(_context.Set<Student>(), "StudentId", "StudentId");
            ViewData["ProfessorId"] = new SelectList(_context.Set<Professor>(), "ProfessorId", "ProfessorId");
            return View();
        }

        // POST: Proposal/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProposalId,Title,Description,Date,State,AccessConditions,Branch,Objectives,ProfessorId,CompanyId,PlacedId")] Proposal proposal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proposal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Set<Company>(), "CompanyId", "Address", proposal.CompanyId);
            ViewData["PlacedId"] = new SelectList(_context.Set<Student>(), "StudentId", "StudentId", proposal.PlacedId);
            ViewData["ProfessorId"] = new SelectList(_context.Set<Professor>(), "ProfessorId", "ProfessorId", proposal.ProfessorId);
            return View(proposal);
        }

        // GET: Proposal/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            ViewData["CompanyId"] = new SelectList(_context.Set<Company>(), "CompanyId", "Address", proposal.CompanyId);
            ViewData["PlacedId"] = new SelectList(_context.Set<Student>(), "StudentId", "StudentId", proposal.PlacedId);
            ViewData["ProfessorId"] = new SelectList(_context.Set<Professor>(), "ProfessorId", "ProfessorId", proposal.ProfessorId);
            return View(proposal);
        }

        // POST: Proposal/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProposalId,Title,Description,Date,State,AccessConditions,Branch,Objectives,ProfessorId,CompanyId,PlacedId")] Proposal proposal)
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
            ViewData["CompanyId"] = new SelectList(_context.Set<Company>(), "CompanyId", "Address", proposal.CompanyId);
            ViewData["PlacedId"] = new SelectList(_context.Set<Student>(), "StudentId", "StudentId", proposal.PlacedId);
            ViewData["ProfessorId"] = new SelectList(_context.Set<Professor>(), "ProfessorId", "ProfessorId", proposal.ProfessorId);
            return View(proposal);
        }

        // GET: Proposal/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Proposal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proposal = await _context.Proposal.FindAsync(id);
            _context.Proposal.Remove(proposal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProposalExists(int id)
        {
            return _context.Proposal.Any(e => e.ProposalId == id);
        }
    }
}
