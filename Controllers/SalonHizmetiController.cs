using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitnessApp.Data;
using FitnessApp.Models;

namespace FitnessApp.Controllers
{
    public class SalonHizmetiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalonHizmetiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SalonHizmeti
        public async Task<IActionResult> Index()
        {
            return View(await _context.SalonHizmetleri.ToListAsync());
        }

        // GET: SalonHizmeti/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salonHizmeti = await _context.SalonHizmetleri
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salonHizmeti == null)
            {
                return NotFound();
            }

            return View(salonHizmeti);
        }

        // GET: SalonHizmeti/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SalonHizmeti/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ad,Sure,Ucret")] SalonHizmeti salonHizmeti)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salonHizmeti);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(salonHizmeti);
        }

        // GET: SalonHizmeti/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salonHizmeti = await _context.SalonHizmetleri.FindAsync(id);
            if (salonHizmeti == null)
            {
                return NotFound();
            }
            return View(salonHizmeti);
        }

        // POST: SalonHizmeti/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ad,Sure,Ucret")] SalonHizmeti salonHizmeti)
        {
            if (id != salonHizmeti.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salonHizmeti);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalonHizmetiExists(salonHizmeti.Id))
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
            return View(salonHizmeti);
        }

        // GET: SalonHizmeti/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salonHizmeti = await _context.SalonHizmetleri
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salonHizmeti == null)
            {
                return NotFound();
            }

            return View(salonHizmeti);
        }

        // POST: SalonHizmeti/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salonHizmeti = await _context.SalonHizmetleri.FindAsync(id);
            if (salonHizmeti != null)
            {
                _context.SalonHizmetleri.Remove(salonHizmeti);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalonHizmetiExists(int id)
        {
            return _context.SalonHizmetleri.Any(e => e.Id == id);
        }
    }
}
