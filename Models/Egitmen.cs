using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Models
{
    public class Egitmen
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [Display(Name = "Ad Soyad")]
        public string AdSoyad { get; set; }

        [Required(ErrorMessage = "Uzmanlık alanı zorunludur.")]
        [Display(Name = "Uzmanlık Alanı")]
        public string UzmanlikAlani { get; set; } // Örn: Kilo Verme, Kas Yapma

        // İlişki: Hangi hizmeti veriyor? (Örn: Yoga hocası)
        [Display(Name = "Verdiği Hizmet")]
        public int SalonHizmetiId { get; set; }
        public virtual SalonHizmeti SalonHizmeti { get; set; }

        // Hocanın istediği "Müsaitlik Saatleri"
        [Required(ErrorMessage = "Çalışma saatleri zorunludur.")]
        [Display(Name = "Çalışma Saatleri")]
        public string CalismaSaatleri { get; set; } // Örn: "09:00-17:00"

        [Display(Name = "Fotoğraf")]
        public string? ResimYolu { get; set; } // Eğitmen fotoğrafı için
    }
}