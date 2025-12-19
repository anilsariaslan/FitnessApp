using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessApp.Data;
using FitnessApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FitnessApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FitnessApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. TÜM EĞİTMENLERİ GETİR (GET: api/FitnessApi/Egitmenler)
        [HttpGet("Egitmenler")]
        public async Task<ActionResult<IEnumerable<object>>> GetEgitmenler()
        {
            // LINQ sorgusu ile sadece gerekli bilgileri seçiyoruz (Hocanın istediği LINQ şartı)
            var egitmenler = await _context.Egitmenler
                .Include(e => e.SalonHizmeti)
                .Select(e => new
                {
                    AdSoyad = e.AdSoyad,
                    Uzmanlik = e.UzmanlikAlani,
                    CalismaSaatleri = e.CalismaSaatleri,
                    Hizmet = e.SalonHizmeti.Ad
                })
                .ToListAsync();

            return Ok(egitmenler);
        }

        // 2. BELLİ BİR HİZMETE GÖRE FİLTRELE (GET: api/FitnessApi/Hizmet/Yoga)
        [HttpGet("Hizmet/{hizmetAdi}")]
        public async Task<ActionResult<IEnumerable<object>>> GetEgitmenlerByHizmet(string hizmetAdi)
        {
            var egitmenler = await _context.Egitmenler
                .Include(e => e.SalonHizmeti)
                .Where(e => e.SalonHizmeti.Ad == hizmetAdi) // Filtreleme (LINQ)
                .Select(e => new
                {
                    AdSoyad = e.AdSoyad,
                    Uzmanlik = e.UzmanlikAlani,
                    Hizmet = e.SalonHizmeti.Ad
                })
                .ToListAsync();

            if (!egitmenler.Any())
            {
                return NotFound("Bu hizmeti veren eğitmen bulunamadı.");
            }

            return Ok(egitmenler);
        }
    }
}