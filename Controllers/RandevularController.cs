using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitnessApp.Data;
using FitnessApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity; // Üyeyi bulmak için lazım

namespace FitnessApp.Controllers
{
    [Authorize] // Sadece giriş yapmış üyeler randevu alabilir
    public class RandevularController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; // Kullanıcı Yöneticisi

        public RandevularController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Randevularımı Listele
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User); // Giriş yapan kullanıcıyı bul

            // Eğer admin ise hepsini görsün, üye ise sadece kendininkileri
            if (User.IsInRole("Admin"))
            {
                var tumRandevular = _context.Randevular.Include(r => r.Egitmen).Include(r => r.Uye);
                return View(await tumRandevular.ToListAsync());
            }
            else
            {
                // Sadece benim randevularım
                var benimRandevularim = _context.Randevular
                    .Include(r => r.Egitmen)
                    .Include(r => r.Uye)
                    .Where(r => r.UyeId == user.Id);

                return View(await benimRandevularim.ToListAsync());
            }
        }

        // ÖZEL RANDEVU ALMA EKRANI (GET)
        // Hangi eğitmene tıklandıysa onun ID'si buraya gelir
        public IActionResult RandevuAl(int? egitmenId)
        {
            if (egitmenId == null) return NotFound();

            var egitmen = _context.Egitmenler.Find(egitmenId);
            ViewBag.EgitmenAd = egitmen.AdSoyad;

            // Modeli hazırlayıp sayfaya gönderiyoruz
            var randevu = new Randevu { EgitmenId = egitmenId.Value };
            return View(randevu);
        }

        // ÖZEL RANDEVU KAYDETME (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RandevuAl([Bind("EgitmenId,Tarih,Saat")] Randevu randevu)
        {
            // Giriş yapan üyeyi bul ve randevuya ekle
            var user = await _userManager.GetUserAsync(User);
            randevu.UyeId = user.Id;
            randevu.OlusturulmaTarihi = DateTime.Now;

            // ModelState validasyonunu biraz gevşetiyoruz (UyeId zorunlu hatası vermesin diye)
            // Çünkü UyeId'yi biz arkada atadık.
            if (randevu.Tarih != DateTime.MinValue)
            {
                _context.Add(randevu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Randevularım sayfasına git
            }

            return View(randevu);
        }

        // --- ADMIN İÇİN OTOMATİK OLUŞAN KISIMLAR (SİLME DETAY VS) ---
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var randevu = await _context.Randevular.Include(r => r.Egitmen).FirstOrDefaultAsync(m => m.Id == id);
            if (randevu == null) return NotFound();
            return View(randevu);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var randevu = await _context.Randevular.FindAsync(id);
            if (randevu != null) _context.Randevular.Remove(randevu);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}