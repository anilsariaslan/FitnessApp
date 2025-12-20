# FitnessApp - Spor Salonu Yönetim Sistemi

## Proje Özeti
Bu proje, Sakarya Üniversitesi Web Programlama dersi kapsamında geliştirilmiş; ASP.NET Core MVC mimarisi üzerine kurulu, Yapay Zeka (Google Gemini) destekli bir spor salonu yönetim ve randevu sistemidir.

Proje; üyelik sistemi, dinamik içerik yönetimi, çakışma kontrollü randevu algoritması ve REST API servisleri içeren tam kapsamlı bir web uygulamasıdır.

---

## Teknik Özellikler

- Yapay Zeka Entegrasyonu: Google Gemini API kullanılarak, üyenin fiziksel verilerine (Boy, Kilo, Hedef) göre kişiselleştirilmiş antrenman programı oluşturan AI modülü.
- Randevu Algoritması: Antrenörlerin müsaitlik durumunu kontrol eden ve aynı saat diliminde çakışan kayıtları engelleyen backend validasyonları.
- REST API & LINQ: Eğitmen ve hizmet verilerinin dışarıya JSON formatında sunulması ve LINQ sorguları ile sunucu tarafında filtreleme yapılması.
- Rol Bazlı Yetkilendirme (RBAC): ASP.NET Core Identity kütüphanesi ile Admin ve Member (Üye) rolleri arasında yetki ayrımı.
- CRUD Operasyonları: Admin paneli üzerinden hizmet, eğitmen ve randevu verilerinin yönetimi.

---

## Teknoloji Yığını (Tech Stack)


| Backend | ASP.NET Core MVC (7.0 / 8.0) |
| Dil | C# |
| Veritabanı | MSSQL (LocalDB) |
| ORM | Entity Framework Core (Code First) |
| AI Servisi | Google Gemini API |
| Frontend | Bootstrap 5, HTML5, CSS3 |

---

## Kurulum ve Çalıştırma

1. Projeyi Klonlayın:
   git clone https://github.com/anilsariaslan/FitnessApp.git

2. Veritabanını Yapılandırın:
   appsettings.json dosyasındaki Connection String bilgisini kontrol edin ve Package Manager Console üzerinden veritabanını oluşturun:
   Update-Database

3. API Anahtarı:
   YapayZekaController.cs içerisindeki _googleApiKey değişkenine geçerli bir Gemini API anahtarı tanımlayın.

4. Çalıştırın:
   Projeyi IIS Express üzerinden başlatın.

---

## Varsayılan Yönetici Bilgileri

Proje isterleri gereği tanımlanan Admin giriş bilgileri:

email:b231210073@sakarya.edu.tr
şifre:sau