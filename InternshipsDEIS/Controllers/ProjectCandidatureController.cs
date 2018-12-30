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
    public class ProjectCandidatureController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectCandidatureController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProjectCandidature
        [Authorize(Roles = "Student,Professor,Administrator")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProjectCandidature.Include(p => p.Candidate).Include(p => p.Project);

            if (User.IsInRole("Student"))
            {
                return View(await applicationDbContext.Where(p => p.Candidate.Id.Equals(GetUserId())).ToListAsync());
            }
            else if (User.IsInRole("Administrator") || User.IsInRole("Committee"))
            {
                return View(await applicationDbContext.ToListAsync());
            }
            else if (User.IsInRole("Professor"))
            {
                return View(await applicationDbContext.Where(p => p.Project.Professors.Any(o => o.Id.Equals(GetUserId()))).ToListAsync());
            }
            else
            {
                return NotFound();
            }
        }

        // GET: ProjectCandidature/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectCandidature = await _context.ProjectCandidature
                .Include(p => p.Candidate)
                .Include(p => p.Project)
                .FirstOrDefaultAsync(m => m.ProjectCandidatureId == id);
            if (projectCandidature == null)
            {
                return NotFound();
            }

            return View(projectCandidature);
        }

        // GET: ProjectCandidature/Create
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Create(string id)
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
            return View();
        }

        // POST: ProjectCandidature/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string id, [Bind("ProjectCandidatureId,StudentId,ProjectId,Branch,Grades,UnfinishedGrades,Result")] ProjectCandidature projectCandidature)
        {
            if (ModelState.IsValid)
            {
                projectCandidature.CandidateId = GetUserId();
                projectCandidature.ProjectId = id;
                _context.Add(projectCandidature);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(projectCandidature);
        }

        // GET: ProjectCandidature/Edit/5
        [Authorize(Roles = "Administrator,Student")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectCandidature = await _context.ProjectCandidature.FindAsync(id);
            if (projectCandidature == null)
            {
                return NotFound();
            }
            return View(projectCandidature);
        }

        // POST: ProjectCandidature/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator,Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ProjectCandidatureId,StudentId,ProjectId,Branch,Grades,UnfinishedGrades,Result")] ProjectCandidature projectCandidature)
        {
            if (id != projectCandidature.ProjectCandidatureId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectCandidature);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectCandidatureExists(projectCandidature.ProjectCandidatureId))
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
            return View(projectCandidature);
        }

        // GET: ProjectCandidature/Delete/5
        [Authorize(Roles = "Administrator,Student")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectCandidature = await _context.ProjectCandidature
                .Include(p => p.Candidate)
                .Include(p => p.Project)
                .FirstOrDefaultAsync(m => m.ProjectCandidatureId == id);
            if (projectCandidature == null)
            {
                return NotFound();
            }

            return View(projectCandidature);
        }

        // POST: ProjectCandidature/Delete/5
        [Authorize(Roles = "Administrator,Student")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var projectCandidature = await _context.ProjectCandidature.FindAsync(id);
            _context.ProjectCandidature.Remove(projectCandidature);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Professor")]
        public async Task<IActionResult> Accept(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectCandidature = await _context.ProjectCandidature
                .Include(p => p.Candidate)
                .Include(p => p.Project)
                .FirstOrDefaultAsync(m => m.ProjectCandidatureId == id);
            if (projectCandidature == null)
            {
                return NotFound();
            }

            var professor = await _context.Users.FindAsync(GetUserId());
            if (professor == null)
            {
                return NotFound();
            }

            // Only professors of project can accept
            if (!projectCandidature.Project.Professors.Contains(professor))
            {
                return NotFound();
            }

            projectCandidature.Result = State.ACCEPTED;

            var project = projectCandidature.Project;
            project.Placed.Add(projectCandidature.Candidate);

            _context.Update(projectCandidature);
            _context.Update(project);
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

            var projectCandidature = await _context.ProjectCandidature
                .Include(p => p.Candidate)
                .Include(p => p.Project)
                .FirstOrDefaultAsync(m => m.ProjectCandidatureId == id);
            if (projectCandidature == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Committee"))
            {
                var professor = await _context.Users.FindAsync(GetUserId());
                if (professor == null)
                {
                    return NotFound();
                }

                // Only professors of project or committee can reject
                if (!projectCandidature.Project.Professors.Contains(professor))
                {
                    return NotFound();
                }
            }

            projectCandidature.Result = State.REJECTED;

            _context.Update(projectCandidature);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ProjectCandidatureExists(string id)
        {
            return _context.ProjectCandidature.Any(e => e.ProjectCandidatureId == id);
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
