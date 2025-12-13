using Microsoft.AspNetCore.Identity;
using FitnessApp.Models;

namespace FitnessApp.Data
{
    public static class DbSeeder
    {
        // Bu metod veritabanını kontrol edip eksik rolleri ve admini ekleyecek
        public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
        {
            // Kullanıcı ve Rol yönetimi servislerini çağırıyoruz
            var userManager = service.GetService<UserManager<IdentityUser>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();

            // 1. ROLLERİ OLUŞTUR (Admin ve Uye)
            // Eğer "Admin" rolü yoksa oluştur
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            // Eğer "Uye" rolü yoksa oluştur
            if (!await roleManager.RoleExistsAsync("Uye"))
                await roleManager.CreateAsync(new IdentityRole("Uye"));

            // 2. ADMİN KULLANICISINI OLUŞTUR
            // Hocanın istediği admin bilgileri
            // DİKKAT: Buradaki maili kendi öğrenci numaranla değiştirmeyi unutma!
            var adminEmail = "b231210073@sakarya.edu.tr";

            // Veritabanında bu mail ile biri var mı?
            var userInDb = await userManager.FindByEmailAsync(adminEmail);

            if (userInDb == null) // Yoksa oluştur
            {
                var adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                // Kullanıcıyı oluştur ve şifresini "sau" yap
                await userManager.CreateAsync(adminUser, "sau");

                // Kullanıcıya "Admin" rolünü ver
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}