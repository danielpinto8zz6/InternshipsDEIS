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
    public class IntershipController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IntershipController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Intership
        public async Task<IActionResult> Index(string search)
        {
            var applicationDbContext = _context.Intership.Include(s => s.Advisor).Include(s => s.Company);

            // TODO: check if professor have rights to accept/reject
            if (User.IsInRole("Committee") || User.IsInRole("Administrator"))
            {
                if (!String.IsNullOrEmpty(search))
                {
                    return View(await applicationDbContext.Where(s => s.Title.Contains(search)).ToListAsync());
                }

                return View(await applicationDbContext.ToListAsync());
            }

            if (!String.IsNullOrEmpty(search))
            {
                return View(await applicationDbContext.Where(s => s.Title.Contains(search) && s.State.Equals(State.ACCEPTED)).ToListAsync());
            }

            // Show only accepted projects to geral/students...
            return View(await applicationDbContext.Where(s => s.State.Equals(State.ACCEPTED)).ToListAsync());
        }

        // GET: Intership/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Intership = await _context.Intership
                .Include(s => s.Advisor)
                .Include(s => s.Company)
                .FirstOrDefaultAsync(m => m.IntershipId == id);
            if (Intership == null)
            {
                return NotFound();
            }

            return View(Intership);
        }

        // GET: Intership/Create
        [Authorize(Roles = "Company")]
        public IActionResult Create()
        {
            ViewData["AdvisorId"] = new SelectList(_context.Users.Where(c => c.Role.Equals("Professor") || c.Role.Equals("Committee")).ToList(), "Id", "UserName");
            return View();
        }

        // POST: Intership/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Company")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IntershipId,Title,Description,Date,State,AccessConditions,Location,Branch,Objectives,AdvisorId,CompanyId")] Intership Intership)
        {
            if (ModelState.IsValid)
            {
                Intership.CompanyId = GetUserId();
                _context.Add(Intership);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["AdvisorId"] = new SelectList(_context.Users.Where(c => c.Role.Equals("Professor") || c.Role.Equals("Committee")).ToList(), "Id", "UserName", Intership.AdvisorId);

            return View(Intership);
        }

        // GET: Intership/Edit/5
        [Authorize(Roles = "Administrator,Company")]
        public async Task<IActionResult> Edit(string id)
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
            ViewData["AdvisorId"] = new SelectList(_context.Users.Where(c => c.Role.Equals("Professor") || c.Role.Equals("Committee")).ToList(), "Id", "UserName", Intership.AdvisorId);
            ViewData["CompanyId"] = new SelectList(_context.Users.Where(c => c.Role.Equals("Company")).ToList(), "Id", "UserName", Intership.CompanyId);
            return View(Intership);
        }

        // POST: Intership/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator,Company")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IntershipId,Title,Description,Date,State,AccessConditions,Location,Branch,Objectives,AdvisorId,CompanyId")] Intership Intership)
        {
            if (id != Intership.IntershipId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Intership);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IntershipExists(Intership.IntershipId))
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
            ViewData["AdvisorId"] = new SelectList(_context.Users.Where(c => c.Role.Equals("Professor") || c.Role.Equals("Committee")).ToList(), "Id", "UserName", Intership.AdvisorId);
            ViewData["CompanyId"] = new SelectList(_context.Users.Where(c => c.Role.Equals("Company")).ToList(), "Id", "UserName", Intership.CompanyId);
            return View(Intership);
        }

        // GET: Intership/Delete/5
        [Authorize(Roles = "Administrator,Company")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Intership = await _context.Intership
                .Include(s => s.Advisor)
                .Include(s => s.Company)
                .FirstOrDefaultAsync(m => m.IntershipId == id);
            if (Intership == null)
            {
                return NotFound();
            }

            return View(Intership);
        }

        // POST: Intership/Delete/5
        [Authorize(Roles = "Administrator,Company")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var Intership = await _context.Intership.FindAsync(id);
            _context.Intership.Remove(Intership);
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

            var Intership = await _context.Intership
                .Include(s => s.Advisor)
                .Include(s => s.Company)
                .FirstOrDefaultAsync(m => m.IntershipId == id);
            if (Intership == null)
            {
                return NotFound();
            }

            var candidature = await _context.IntershipCandidature
                .FirstOrDefaultAsync(m => m.IntershipId == id && m.Candidate.Id == GetUserId());

            if (candidature != null)
            {
                return RedirectToAction("Details", "IntershipCandidature", new { id = candidature.IntershipCandidatureId });
            }

            return RedirectToAction("Create", "IntershipCandidature", new { id = id });
        }

        [Authorize(Roles = "Committee")]
        public async Task<IActionResult> Accept(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var intership = await _context.Intership.FindAsync(id);
            if (intership == null)
            {
                return NotFound();
            }

            // Accept
            intership.State = State.ACCEPTED;

            _context.Update(intership);
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

            var intership = await _context.Intership.FindAsync(id);
            if (intership == null)
            {
                return NotFound();
            }

            // Accept
            intership.State = State.REJECTED;

            _context.Update(intership);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool IntershipExists(string id)
        {
            return _context.Intership.Any(e => e.IntershipId == id);
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
