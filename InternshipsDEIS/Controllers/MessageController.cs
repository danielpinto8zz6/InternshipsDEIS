using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InternshipsDEIS.Data;
using InternshipsDEIS.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace InternshipsDEIS.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MessageController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Message
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return NotFound();
            }

            if (!User.IsInRole("Administrator"))
            {
                return RedirectToAction(nameof(Received));
            }

            var applicationDbContext = _context.Message.Include(m => m.Recipient).Include(m => m.Sender);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Message
        public async Task<IActionResult> Sent()
        {
            var applicationDbContext = _context.Message.Include(m => m.Recipient).Include(m => m.Sender);
            var messages = applicationDbContext.Where(m => m.SenderId.Equals(GetUserId()));
            return View(await messages.ToListAsync());
        }

        // GET: Message
        public async Task<IActionResult> Received()
        {
            var applicationDbContext = _context.Message.Include(m => m.Recipient).Include(m => m.Sender);
            var messages = applicationDbContext.Where(m => m.RecipientId.Equals(GetUserId()));
            return View(await messages.ToListAsync());
        }

        // GET: Message/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message
                .Include(m => m.Recipient)
                .Include(m => m.Sender)
                .FirstOrDefaultAsync(m => m.MessageId == id);
            if (message == null)
            {
                return NotFound();
            }

            var userId = GetUserId();

            // You can only see your messages
            if (message.RecipientId.Equals(userId) || message.SenderId.Equals(userId))
            {
                if (message.RecipientId.Equals(userId))
                {
                    if (!message.read)
                    {
                        message.read = true;
                        await _context.SaveChangesAsync();
                    }
                }

                return View(message);
            }

            return NotFound();
        }

        // GET: Message/Create
        public IActionResult Create()
        {
            ViewData["RecipientId"] = new SelectList(_context.Users.Where(u => u.Id != GetUserId()), "Id", "UserName");
            return View();
        }

        // POST: Message/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MessageId,SenderId,RecipientId,Title,Text,Date")] Message message)
        {
            if (ModelState.IsValid)
            {
                message.SenderId = GetUserId();
                message.Date = DateTime.UtcNow;
                _context.Add(message);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RecipientId"] = new SelectList(_context.Users.Where(u => u.Id != GetUserId()), "Id", "Id", message.RecipientId);
            return View(message);
        }

        // GET: Message/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message
                .Include(m => m.Recipient)
                .Include(m => m.Sender)
                .FirstOrDefaultAsync(m => m.MessageId == id);
            if (message == null)
            {
                return NotFound();
            }

            var userId = GetUserId();

            // You can only delete your messages
            if (message.RecipientId.Equals(userId) || message.SenderId.Equals(userId))
            {
                return View(message);
            }

            return NotFound();
        }

        // POST: Message/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var message = await _context.Message.FindAsync(id);
            _context.Message.Remove(message);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Message/Reply/5
        public async Task<IActionResult> Reply(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message
                .Include(m => m.Recipient)
                .Include(m => m.Sender)
                .FirstOrDefaultAsync(m => m.MessageId == id);
            if (message == null)
            {
                return NotFound();
            }

            var userId = GetUserId();
            if (!message.RecipientId.Equals(userId))
            {
                return NotFound();
            }

            ViewData["RecipientId"] = new SelectList(_context.Users, "Id", "UserName", message.SenderId);
            return View();
        }

        private bool MessageExists(string id)
        {
            return _context.Message.Any(e => e.MessageId == id);
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
