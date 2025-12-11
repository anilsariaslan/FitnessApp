using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FitnessApp.Models
{
    public class Randevu
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Randevu Tarihi")]
        [DataType(DataType.Date)]
        public DateTime Tarih { get; set; }

        [Required]
        [Display(Name = "Randevu Saati")]
        [DataType(DataType.Time)]
        public TimeSpan Saat { get; set; }

        public DateTime OlusturulmaTarihi { get; set; } = DateTime.Now;

        // İlişki: Randevuyu alan Üye (IdentityUser kullanıyoruz)
        public string UyeId { get; set; }
        public virtual IdentityUser Uye { get; set; }

        // İlişki: Randevu alınan Eğitmen
        public int EgitmenId { get; set; }
        public virtual Egitmen Egitmen { get; set; }
    }
}