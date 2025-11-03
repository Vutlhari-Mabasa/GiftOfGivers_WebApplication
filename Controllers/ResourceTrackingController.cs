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
    public class ResourceTrackingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResourceTrackingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ResourceTracking
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ResourceTracking.Include(r => r.ReliefProject);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ResourceTracking/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resourceTracking = await _context.ResourceTracking
                .Include(r => r.ReliefProject)
                .FirstOrDefaultAsync(m => m.ResourceID == id);
            if (resourceTracking == null)
            {
                return NotFound();
            }

            return View(resourceTracking);
        }

        // GET: ResourceTracking/Create
        public IActionResult Create()
        {
            ViewData["ReliefProjectID"] = new SelectList(_context.ReliefProjects, "ReliefProjectID", "Name");
            return View();
        }

        // POST: ResourceTracking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReliefProjectID,Name,Quantity,Unit,Category,Description,DonatedBy,DonationDate,Priority,Location,ExpiryDate,Notes")] ResourceTracking resourceTracking)
        {
            if (ModelState.IsValid)
            {
                resourceTracking.Status = "Available";
                _context.Add(resourceTracking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ReliefProjectID"] = new SelectList(_context.ReliefProjects, "ReliefProjectID", "Name", resourceTracking.ReliefProjectID);
            return View(resourceTracking);
        }



        // GET: ResourceTracking/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resourceTracking = await _context.ResourceTracking.FindAsync(id);
            if (resourceTracking == null)
            {
                return NotFound();
            }
            ViewData["ReliefProjectID"] = new SelectList(_context.ReliefProjects, "ReliefProjectID", "Name", resourceTracking.ReliefProjectID);
            return View(resourceTracking);
        }

        // POST: ResourceTracking/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ResourceID,ReliefProjectID,Name,Quantity,Unit,Category,Description,DonatedBy,DonationDate,Priority,Location,ExpiryDate,Notes,Status")] ResourceTracking resourceTracking)
        {
            if (id != resourceTracking.ResourceID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(resourceTracking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResourceTrackingExists(resourceTracking.ResourceID))
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
            ViewData["ReliefProjectID"] = new SelectList(_context.ReliefProjects, "ReliefProjectID", "Name", resourceTracking.ReliefProjectID);
            return View(resourceTracking);
        }

        // GET: ResourceTracking/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resourceTracking = await _context.ResourceTracking
                .Include(r => r.ReliefProject)
                .FirstOrDefaultAsync(m => m.ResourceID == id);
            if (resourceTracking == null)
            {
                return NotFound();
            }

            return View(resourceTracking);
        }

        // POST: ResourceTracking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var resourceTracking = await _context.ResourceTracking.FindAsync(id);
            if (resourceTracking != null)
            {
                _context.ResourceTracking.Remove(resourceTracking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResourceTrackingExists(int id)
        {
            return _context.ResourceTracking.Any(e => e.ResourceID == id);
        }
    }
}
