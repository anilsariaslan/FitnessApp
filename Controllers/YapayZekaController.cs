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
                // 1. GERÇEK AI DENEMESİ
                sonuc = await GeminiyeSor(boy, kilo, cinsiyet, hedef, "gemini-2.0-flash");
            }
            catch (Exception)
            {
                // 2. YEDEK PLAN (İNTERNET GİDERSE BURASI ÇALIŞIR)
                sonuc = SimuleEdilmisZekaCevabi(boy, kilo, cinsiyet, hedef);
            }

            ViewBag.Sonuc = sonuc;
            return View("Index");
        }

        // --- GERÇEK AI ---
        private async Task<string> GeminiyeSor(int boy, int kilo, string cinsiyet, string hedef, string modelAdi)
        {
            using (var client = new HttpClient())
            {
                string url = $"https://generativelanguage.googleapis.com/v1beta/models/{modelAdi}:generateContent?key={_googleApiKey}";

                // GÜNCELLEME: Prompt içinde sayı vermesini serbest bıraktık ama "imza atma" dedik.
                var prompt = $"Sen profesyonel bir spor koçusun. Kullanıcı: Boy {boy}cm, Kilo {kilo}kg, Cinsiyet {cinsiyet}, Hedef {hedef}. " +
                             $"Bu kişiye VKİ hesapla, durumunu yorumla ve maddeler halinde diyet/spor tavsiyesi ver. " +
                             $"Sayısal değerler (süre, set sayısı vb.) kullanabilirsin. " +
                             $"Çok ciddi ve kurumsal bir dil kullan. Asla emoji kullanma. " +
                             $"ÖNEMLİ: Cevabın sonuna 'Saygılar', 'Asistan', 'Yapay Zeka' gibi hiçbir kapanış cümlesi veya imza EKLEME. Sadece tavsiyeleri yaz ve bitir.";

                var requestBody = new { contents = new[] { new { parts = new[] { new { text = prompt } } } } };
                var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, jsonContent);
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                var jsonNode = JsonNode.Parse(responseString);

                string? aiCevabi = jsonNode?["candidates"]?[0]?["content"]?[0]?["parts"]?[0]?["text"]?.ToString();

                if (string.IsNullOrEmpty(aiCevabi)) throw new Exception("Boş");

                // GÜNCELLEME: Buradaki ekleme kodunu kaldırdık. Artık sadece AI cevabı dönüyor.
                return aiCevabi;
            }
        }

        // --- YEDEK PLAN (SİMÜLASYON) ---
        private string SimuleEdilmisZekaCevabi(int boy, int kilo, string cinsiyet, string hedef)
        {
            double boyM = (double)boy / 100;
            double vki = kilo / (boyM * boyM);
            string durum = vki < 25 ? "İdeal Kilo" : (vki < 30 ? "Hafif Kilolu" : "Obezite Riski");

            string tavsiye = "";
            if (hedef.Contains("Kilo")) tavsiye = "- Günde 45 dk tempolu yürüyüş yapın.\n- Akşam 7'den sonra karbonhidrat tüketmeyin.\n- Şekerli içecekleri hayatınızdan çıkarın.";
            else if (hedef.Contains("Kas")) tavsiye = "- Ağırlık antrenmanlarına odaklanın (Haftada 4 gün).\n- Günlük protein alımını artırın.\n- Antrenman sonrası muz ve süt tüketin.";
            else tavsiye = "- Günde en az 2.5 litre su için.\n- Haftada 3 gün egzersiz yapın.\n- İşlenmiş gıdalardan uzak durun.";

            // GÜNCELLEME: Buradaki sondaki "FitnessApp Algoritması..." yazısını kaldırdık.
            return $"Sayın üyemiz, fiziksel analiziniz tamamlandı.\n\n" +
                   $"Vücut Kitle İndeksi: {vki:F1} ({durum})\n" +
                   $"Belirlenen Hedef: {hedef}\n\n" +
                   $"Uzman Tavsiyesi:\n{tavsiye}";
        }
    }
}