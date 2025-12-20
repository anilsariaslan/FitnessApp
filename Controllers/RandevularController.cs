using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitnessApp.Data;
using FitnessApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace FitnessApp.Controllers
{
    [Authorize] // Sadece giriş yapmış üyeler randevu alabilir
    public class RandevularController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public RandevularController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // 1. RANDEVULARI LİSTELE (Sadece kendi randevularım)
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            // Eğer kullanıcı oturumu düşmüşse login'e at
            if (user == null) return RedirectToAction("Login", "Account");

            var randevular = await _context.Randevular
                .Include(r => r.Egitmen) // Eğitmen adını da getir
                .Where(r => r.UyeId == user.Id) // Sadece BENİM randevularım
                .OrderByDescending(r => r.Tarih) // En yeni en üstte
                .ToListAsync();

            return View(randevular);
        }

        // 2. RANDEVU ALMA SAYFASINI AÇ (GET)
        // Tarayıcıda .../Randevular/Create adresine gidince burası çalışır
        public IActionResult Create()
        {
            // Eğitmen listesini veritabanından çekip sayfaya gönderiyoruz
            ViewData["EgitmenId"] = new SelectList(_context.Egitmenler, "Id", "AdSoyad");
            return View();
        }

        // 3. RANDEVUYU KAYDET (POST)
        // "Randevu Al" butonuna basınca burası çalışır
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EgitmenId,Tarih,Saat")] Randevu randevu)
        {
            // Giriş yapan kullanıcıyı bul
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                randevu.UyeId = user.Id;
            }

            randevu.OlusturulmaTarihi = DateTime.Now;
            ModelState.Remove("UyeId");
            ModelState.Remove("Uye");
            ModelState.Remove("Egitmen");

            // --- 🛑 ÇAKIŞMA KONTROLÜ (EN ÖNEMLİ KISIM) ---
            // Seçilen eğitmenin, o gün ve o saatte başka randevusu var mı?
            bool cakismaVar = await _context.Randevular.AnyAsync(r =>
                r.EgitmenId == randevu.EgitmenId &&
                r.Tarih == randevu.Tarih &&
                r.Saat == randevu.Saat);

            if (cakismaVar)
            {
                // HATA VARSA: Kullanıcıya uyarı göster
                ModelState.AddModelError("", "⚠️ Üzgünüz, bu eğitmenin seçtiğiniz saatte başka bir randevusu var. Lütfen başka bir saat seçiniz.");

                // Sayfayı (Dropdown dolu şekilde) tekrar yükle
                ViewData["EgitmenId"] = new SelectList(_context.Egitmenler, "Id", "AdSoyad", randevu.EgitmenId);
                return View(randevu);
            }
            // ----------------------------------------------

            // Geçmişe randevu alınmasını engelle (Opsiyonel ama iyi olur)
            if (randevu.Tarih < DateTime.Today)
            {
                ModelState.AddModelError("", "Geçmiş bir tarihe randevu alamazsınız.");
                ViewData["EgitmenId"] = new SelectList(_context.Egitmenler, "Id", "AdSoyad", randevu.EgitmenId);
                return View(randevu);
            }

            // Her şey yolundaysa kaydet
            if (ModelState.IsValid)
            {
                _context.Add(randevu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Listeye geri dön
            }

            // Model geçerli değilse sayfayı tekrar göster
            ViewData["EgitmenId"] = new SelectList(_context.Egitmenler, "Id", "AdSoyad", randevu.EgitmenId);
            return View(randevu);
        }

        // 4. RANDEVU SİL / İPTAL ET
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var randevu = await _context.Randevular.FindAsync(id);
            if (randevu != null)
            {
                _context.Randevular.Remove(randevu);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}