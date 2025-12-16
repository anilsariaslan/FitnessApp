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
    public class EgitmenController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EgitmenController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Egitmen
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Egitmenler.Include(e => e.SalonHizmeti);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Egitmen/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var egitmen = await _context.Egitmenler
                .Include(e => e.SalonHizmeti)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (egitmen == null)
            {
                return NotFound();
            }

            return View(egitmen);
        }

        // GET: Egitmen/Create
        public IActionResult Create()
        {
            ViewData["SalonHizmetiId"] = new SelectList(_context.SalonHizmetleri, "Id", "Ad");
            return View();
        }

        // POST: Egitmen/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AdSoyad,UzmanlikAlani,SalonHizmetiId,CalismaSaatleri,ResimYolu")] Egitmen egitmen)
        {
            if (ModelState.IsValid)
            {
                _context.Add(egitmen);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SalonHizmetiId"] = new SelectList(_context.SalonHizmetleri, "Id", "Ad", egitmen.SalonHizmetiId);
            return View(egitmen);
        }

        // GET: Egitmen/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var egitmen = await _context.Egitmenler.FindAsync(id);
            if (egitmen == null)
            {
                return NotFound();
            }
            ViewData["SalonHizmetiId"] = new SelectList(_context.SalonHizmetleri, "Id", "Ad", egitmen.SalonHizmetiId);
            return View(egitmen);
        }

        // POST: Egitmen/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AdSoyad,UzmanlikAlani,SalonHizmetiId,CalismaSaatleri,ResimYolu")] Egitmen egitmen)
        {
            if (id != egitmen.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(egitmen);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EgitmenExists(egitmen.Id))
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
            ViewData["SalonHizmetiId"] = new SelectList(_context.SalonHizmetleri, "Id", "Ad", egitmen.SalonHizmetiId);
            return View(egitmen);
        }

        // GET: Egitmen/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var egitmen = await _context.Egitmenler
                .Include(e => e.SalonHizmeti)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (egitmen == null)
            {
                return NotFound();
            }

            return View(egitmen);
        }

        // POST: Egitmen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var egitmen = await _context.Egitmenler.FindAsync(id);
            if (egitmen != null)
            {
                _context.Egitmenler.Remove(egitmen);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EgitmenExists(int id)
        {
            return _context.Egitmenler.Any(e => e.Id == id);
        }
    }
}
