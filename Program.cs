using FitnessApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Identity ayarlarý (Burasý KESÝN böyle olmalý)
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true; // Mail onayý istersen true kalsýn
    options.Password.RequireDigit = false;         // Þifre zorluk ayarlarý (Ýstersen gevþetebilirsin)
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 3; // Þifre en az 3 hane olsun (sau þifresi için)
})
.AddRoles<IdentityRole>() // <--- Rol mekanizmasýný açan kilit kod
.AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
// --- SEED DATA (Otomatik Veri) BAÞLANGICI ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    // DbSeeder sýnýfýndaki metodu çalýþtýr
    await DbSeeder.SeedRolesAndAdminAsync(services);
}
// --- SEED DATA BÝTÝÞÝ ---

app.Run();
