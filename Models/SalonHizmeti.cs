using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Models
{
    public class SalonHizmeti
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Hizmet adı zorunludur.")]
        [Display(Name = "Hizmet Adı")]
        public string Ad { get; set; } // Örn: Yoga, Pilates

        [Display(Name = "Süre (Dakika)")]
        [Required(ErrorMessage = "Süre girilmesi zorunludur.")]
        public int Sure { get; set; }

        [Display(Name = "Ücret (TL)")]
        [Required(ErrorMessage = "Ücret girilmesi zorunludur.")]
        [Column(TypeName = "decimal(18,2)")]//18 basamak, 2'si virgülden sonra
        public decimal Ucret { get; set; }

        // İlişki: Bu hizmeti veren eğitmenler
        public virtual ICollection<Egitmen> Egitmenler { get; set; }
    }
}   