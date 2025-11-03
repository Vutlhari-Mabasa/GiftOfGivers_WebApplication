using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GiftOfGivers_WebApplication.Data;
using GiftOfGivers_WebApplication.Models;

namespace GiftOfGivers_WebApplication.Controllers
{
    [Authorize]
    public class VolunteersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public VolunteersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Volunteers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Volunteers.Include(v => v.User).ToListAsync());
        }

        // GET: Volunteers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteer = await _context.Volunteers
                .Include(v => v.User)
                .Include(v => v.VolunteerAssignments)
                    .ThenInclude(va => va.VolunteerTask)
                .FirstOrDefaultAsync(m => m.VolunteerID == id);

            if (volunteer == null)
            {
                return NotFound();
            }

            return View(volunteer);
        }

        // GET: Volunteers/Register
        public async Task<IActionResult> Register()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Check if user already has a volunteer profile
            var existingVolunteer = await _context.Volunteers
                .FirstOrDefaultAsync(v => v.UserId == user.Id);

            if (existingVolunteer != null)
            {
                return RedirectToAction("Details", new { id = existingVolunteer.VolunteerID });
            }

            return View();
        }

        // POST: Volunteers/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("FirstName,LastName,Email,Phone,Address,City,Skills,AvailableDays,Notes")] Volunteer volunteer)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Check if user already has a volunteer profile
            var existingVolunteer = await _context.Volunteers
                .FirstOrDefaultAsync(v => v.UserId == user.Id);

            if (existingVolunteer != null)
            {
                return RedirectToAction("Details", new { id = existingVolunteer.VolunteerID });
            }

            if (ModelState.IsValid)
            {
                volunteer.UserId = user.Id;
                volunteer.RegistrationDate = DateTime.Now;
                volunteer.Status = "Available";

                _context.Add(volunteer);
                await _context.SaveChangesAsync();
                return RedirectToAction("Dashboard", "Volunteers");
            }

            return View(volunteer);
        }

        // GET: Volunteers/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var volunteer = await _context.Volunteers
                .FirstOrDefaultAsync(v => v.UserId == user.Id);

            if (volunteer == null)
            {
                return RedirectToAction("Register");
            }

            var assignments = await _context.VolunteerAssignments
                .Include(va => va.VolunteerTask)
                    .ThenInclude(vt => vt.ReliefProject)
                .Where(va => va.VolunteerID == volunteer.VolunteerID)
                .OrderByDescending(va => va.AssignedDate)
                .Take(10)
                .ToListAsync();

            var openTasks = await _context.VolunteerTasks
                .Include(vt => vt.ReliefProject)
                .Where(vt => vt.Status == "Open")
                .OrderByDescending(vt => vt.CreatedAt)
                .Take(10)
                .ToListAsync();

            ViewBag.Assignments = assignments;
            ViewBag.OpenTasks = openTasks;

            return View(volunteer);
        }

        // GET: Volunteers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteer = await _context.Volunteers.FindAsync(id);
            if (volunteer == null)
            {
                return NotFound();
            }
            return View(volunteer);
        }

        // POST: Volunteers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VolunteerID,FirstName,LastName,Email,Phone,Address,City,Skills,AvailableDays,Status,Notes")] Volunteer volunteer)
        {
            if (id != volunteer.VolunteerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(volunteer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VolunteerExists(volunteer.VolunteerID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = volunteer.VolunteerID });
            }
            return View(volunteer);
        }

        // GET: Volunteers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteer = await _context.Volunteers
                .Include(v => v.User)
                .FirstOrDefaultAsync(m => m.VolunteerID == id);

            if (volunteer == null)
            {
                return NotFound();
            }

            return View(volunteer);
        }

        // POST: Volunteers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var volunteer = await _context.Volunteers.FindAsync(id);
            if (volunteer != null)
            {
                _context.Volunteers.Remove(volunteer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VolunteerExists(int id)
        {
            return _context.Volunteers.Any(e => e.VolunteerID == id);
        }
    }
}

