using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OganiMasterMVC.Areas.Admin.Data;
using OganiMasterMVC.Data.DataContext;
namespace OganiMasterMVC
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var con = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(con));

            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            builder.Services.AddScoped<DataInit>();
            PathConstants.ProductImagePath = Path.Combine(builder.Environment.WebRootPath, "Admin", "images", "product");
            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var dataInit = scope.ServiceProvider.GetRequiredService<DataInit>();
                await dataInit.SeedData();
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
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
                name: "areas",
                pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            await app.RunAsync();
        }
    }
}