using FitnessApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tablolarımız
        public DbSet<SalonHizmeti> SalonHizmetleri { get; set; }
        public DbSet<Egitmen> Egitmenler { get; set; }
        public DbSet<Randevu> Randevular { get; set; }
    }
}