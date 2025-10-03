using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GiftOfGivers_WebApplication.Data;
using GiftOfGivers_WebApplication.Models;

namespace GiftOfGivers_WebApplication.Controllers
{
    public class IncidentReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IncidentReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: IncidentReports
        public async Task<IActionResult> Index()
        {
            return View(await _context.IncidentReports.ToListAsync());
        }

        // GET: IncidentReports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incidentReport = await _context.IncidentReports
                .FirstOrDefaultAsync(m => m.IncidentID == id);
            if (incidentReport == null)
            {
                return NotFound();
            }

            return View(incidentReport);
        }

        // GET: IncidentReports/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: IncidentReports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IncidentID,Name,Type,Location,StartDate")] IncidentReport incidentReport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(incidentReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(incidentReport);
        }

        // GET: IncidentReports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incidentReport = await _context.IncidentReports.FindAsync(id);
            if (incidentReport == null)
            {
                return NotFound();
            }
            return View(incidentReport);
        }

        // POST: IncidentReports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IncidentID,Name,Type,Location,StartDate")] IncidentReport incidentReport)
        {
            if (id != incidentReport.IncidentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(incidentReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncidentReportExists(incidentReport.IncidentID))
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
            return View(incidentReport);
        }

        // GET: IncidentReports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incidentReport = await _context.IncidentReports
                .FirstOrDefaultAsync(m => m.IncidentID == id);
            if (incidentReport == null)
            {
                return NotFound();
            }

            return View(incidentReport);
        }

        // POST: IncidentReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var incidentReport = await _context.IncidentReports.FindAsync(id);
            if (incidentReport != null)
            {
                _context.IncidentReports.Remove(incidentReport);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IncidentReportExists(int id)
        {
            return _context.IncidentReports.Any(e => e.IncidentID == id);
        }
    }
}