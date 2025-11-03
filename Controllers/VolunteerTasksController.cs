using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GiftOfGivers_WebApplication.Data;
using GiftOfGivers_WebApplication.Models;

namespace GiftOfGivers_WebApplication.Controllers
{
    [Authorize]
    public class VolunteerTasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VolunteerTasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: VolunteerTasks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.VolunteerTasks.Include(v => v.ReliefProject);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: VolunteerTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteerTask = await _context.VolunteerTasks
                .Include(v => v.ReliefProject)
                .Include(v => v.VolunteerAssignments)
                    .ThenInclude(va => va.Volunteer)
                .FirstOrDefaultAsync(m => m.TaskID == id);

            if (volunteerTask == null)
            {
                return NotFound();
            }

            return View(volunteerTask);
        }

        // GET: VolunteerTasks/Create
        public IActionResult Create()
        {
            ViewData["ReliefProjectID"] = new SelectList(_context.ReliefProjects, "ReliefProjectID", "Name");
            return View();
        }

        // POST: VolunteerTasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,RequiredSkills,ReliefProjectID,Priority,Location,StartDate,EndDate,VolunteersNeeded")] VolunteerTask volunteerTask)
        {
            if (ModelState.IsValid)
            {
                volunteerTask.Status = "Open";
                volunteerTask.CreatedAt = DateTime.Now;
                _context.Add(volunteerTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ReliefProjectID"] = new SelectList(_context.ReliefProjects, "ReliefProjectID", "Name", volunteerTask.ReliefProjectID);
            return View(volunteerTask);
        }

        // GET: VolunteerTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteerTask = await _context.VolunteerTasks.FindAsync(id);
            if (volunteerTask == null)
            {
                return NotFound();
            }
            ViewData["ReliefProjectID"] = new SelectList(_context.ReliefProjects, "ReliefProjectID", "Name", volunteerTask.ReliefProjectID);
            return View(volunteerTask);
        }

        // POST: VolunteerTasks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaskID,Title,Description,RequiredSkills,ReliefProjectID,Priority,Status,Location,StartDate,EndDate,VolunteersNeeded")] VolunteerTask volunteerTask)
        {
            if (id != volunteerTask.TaskID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(volunteerTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VolunteerTaskExists(volunteerTask.TaskID))
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
            ViewData["ReliefProjectID"] = new SelectList(_context.ReliefProjects, "ReliefProjectID", "Name", volunteerTask.ReliefProjectID);
            return View(volunteerTask);
        }

        // GET: VolunteerTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteerTask = await _context.VolunteerTasks
                .Include(v => v.ReliefProject)
                .FirstOrDefaultAsync(m => m.TaskID == id);

            if (volunteerTask == null)
            {
                return NotFound();
            }

            return View(volunteerTask);
        }

        // POST: VolunteerTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var volunteerTask = await _context.VolunteerTasks.FindAsync(id);
            if (volunteerTask != null)
            {
                _context.VolunteerTasks.Remove(volunteerTask);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: VolunteerTasks/Assign/5
        public async Task<IActionResult> Assign(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteerTask = await _context.VolunteerTasks.FindAsync(id);
            if (volunteerTask == null)
            {
                return NotFound();
            }

            ViewData["VolunteerID"] = new SelectList(_context.Volunteers, "VolunteerID", "FullName");
            return View(new VolunteerAssignment { TaskID = id.Value });
        }

        // POST: VolunteerTasks/Assign/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(int id, [Bind("VolunteerID,TaskID,Notes")] VolunteerAssignment assignment)
        {
            if (ModelState.IsValid)
            {
                assignment.Status = "Assigned";
                assignment.AssignedDate = DateTime.Now;
                _context.Add(assignment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = id });
            }

            ViewData["VolunteerID"] = new SelectList(_context.Volunteers, "VolunteerID", "FullName");
            return View(assignment);
        }

        private bool VolunteerTaskExists(int id)
        {
            return _context.VolunteerTasks.Any(e => e.TaskID == id);
        }
    }
}

