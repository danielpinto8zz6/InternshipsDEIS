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
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Project
        public async Task<IActionResult> Index(string search)
        {
            // TODO: check if professor have rights to accept/reject
            if (User.IsInRole("Committee") || User.IsInRole("Administrator"))
            {

                if (!String.IsNullOrEmpty(search))
                {
                    var filter = _context.Project.Where(s => s.Title.Contains(search));
                    return View(await filter.ToListAsync());
                }

                return View(await _context.Project.ToListAsync());
            }

            if (User.IsInRole("Professor"))
            {

                if (!String.IsNullOrEmpty(search))
                {
                    var filter = _context.Project.Where(s => s.Title.Contains(search));
                    return View(await filter.Where(p => p.Professors.FirstOrDefault().Id.Equals(GetUserId())).ToListAsync());
                }
                return View(await _context.Project.Where(p => p.Professors.FirstOrDefault().Id.Equals(GetUserId())).ToListAsync());
            }

            if (!String.IsNullOrEmpty(search))
            {
                var filter = _context.Project.Where(p => p.State.Equals(State.ACCEPTED) && p.Title.Contains(search));
                return View(await filter.ToListAsync());
            }

            // Show only accepted projects to geral/students...
            return View(await _context.Project.Where(p => p.State.Equals(State.ACCEPTED)).ToListAsync());
        }

        // GET: Project/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        [Authorize(Roles = "Professor")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Professor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectId,Title,Description,Date,State,AccessConditions,Branch,Objectives")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.Date = DateTime.UtcNow.Date;

                var professor = await _context.Users.FindAsync(GetUserId());
                if (professor == null)
                {
                    return NotFound();
                }

                project.Professors.Add(professor);
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: Project/Edit/5
        [Authorize(Roles = "Administrator,Professor")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator,Professor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ProjectId,Title,Description,Date,State,AccessConditions,Branch,Objectives")] Project project)
        {
            if (id != project.ProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ProjectId))
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
            return View(project);
        }

        // GET: Project/Delete/5
        [Authorize(Roles = "Administrator,Professor")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Project/Delete/5
        [Authorize(Roles = "Administrator,Professor")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var project = await _context.Project.FindAsync(id);
            _context.Project.Remove(project);
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

            var project = await _context.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            var candidature = await _context.ProjectCandidature
                .FirstOrDefaultAsync(m => m.ProjectId == id && m.Candidate.Id == GetUserId());

            if (candidature != null)
            {
                return RedirectToAction("Details", "ProjectCandidature", new { id = candidature.ProjectCandidatureId });
            }

            return RedirectToAction("Create", "ProjectCandidature", new { id = id });
        }

        [Authorize(Roles = "Committee")]
        public async Task<IActionResult> Accept(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            // Accept
            project.State = State.ACCEPTED;

            _context.Update(project);
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

            var project = await _context.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            // Accept
            project.State = State.REJECTED;

            _context.Update(project);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(string id)
        {
            return _context.Project.Any(e => e.ProjectId == id);
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
