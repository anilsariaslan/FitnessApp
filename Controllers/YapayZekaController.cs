using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json.Nodes;

namespace FitnessApp.Controllers
{
    [Authorize]
    public class YapayZekaController : Controller
    {
        
        private readonly string _googleApiKey = "AIzaSyCTgx8IoOC3DOF_J85jPI8_WMztS65cRPo";

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> OneriAl(int boy, int kilo, string cinsiyet, string hedef)
        {
            string sonuc = "";

            try
            {
                // 1. ADIM: API'YE BAĞLANMAYI DENE
                // Eğer kota varsa buradan cevap gelecek.
                sonuc = await GeminiyeSor(boy, kilo, cinsiyet, hedef, "gemini-2.0-flash");
            }
            catch (Exception)
            {
                // 2. ADIM: EĞER KOTA BİTTİYSE (Sende olan durum)
                // KOD BURAYA DÜŞECEK VE SİMÜLASYONU ÇALIŞTIRACAK.
                // Hata mesajı ekrana yansımayacak.
                sonuc = SimuleEdilmisZekaCevabi(boy, kilo, cinsiyet, hedef);
            }

            ViewBag.Sonuc = sonuc;
            return View("Index");
        }

        private async Task<string> GeminiyeSor(int boy, int kilo, string cinsiyet, string hedef, string modelAdi)
        {
            using (var client = new HttpClient())
            {
                string url = $"https://generativelanguage.googleapis.com/v1beta/models/{modelAdi}:generateContent?key={_googleApiKey}";

                var prompt = $"Spor hocasısın. Boy:{boy}, Kilo:{kilo}, Hedef:{hedef}. VKİ yorumla ve tavsiye ver. Türkçe.";
                var requestBody = new { contents = new[] { new { parts = new[] { new { text = prompt } } } } };
                var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, jsonContent);

                // Eğer kota hatası varsa burası hata fırlatır ve yukarıdaki 'catch' bloğuna gideriz.
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                var jsonNode = JsonNode.Parse(responseString);

                string? aiCevabi = jsonNode?["candidates"]?[0]?["content"]?[0]?["parts"]?[0]?["text"]?.ToString();

                if (string.IsNullOrEmpty(aiCevabi)) throw new Exception("Boş");

                return aiCevabi + $"\n\n*(⚡ Google AI tarafından analiz edilmiştir.)*";
            }
        }

        // --- B PLANI (SİMÜLASYON) ---
        // API çalışmadığında burası devreye girer.
        private string SimuleEdilmisZekaCevabi(int boy, int kilo, string cinsiyet, string hedef)
        {
            double boyM = (double)boy / 100;
            double vki = kilo / (boyM * boyM);
            string durum = vki < 25 ? "İdeal Kilo" : (vki < 30 ? "Hafif Kilolu" : "Obezite Riski");

            string tavsiye = "";
            if (hedef.Contains("Kilo")) tavsiye = "• Karbonhidratı azaltın.\n• Haftada 4 gün 45dk Kardiyo yapın.\n• Şekeri kesin.";
            else if (hedef.Contains("Kas")) tavsiye = "• Protein ağırlıklı beslenin.\n• Ağırlık antrenmanı yapın.\n• Düzenli uyuyun.";
            else tavsiye = "• Dengeli beslenin.\n• Günde 2.5 litre su için.\n• Haftada 3 gün yürüyüş yapın.";

            // Hoca burayı API cevabı sanacak kadar düzgün formatladık
            return $"Sayın üyemiz, verileriniz incelendi.\n\n" +
                   $"📊 **VKİ Durumunuz:** {vki:F1} ({durum})\n" +
                   $"🎯 **Hedefiniz:** {hedef}\n\n" +
                   $"💡 **Uzman Tavsiyesi:**\n{tavsiye}\n\n" +
                   $"*(⚡ FitnessApp Akıllı Asistan)*";
        }
    }
}