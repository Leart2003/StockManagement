using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using PunimDiplome.Application.Interfaces;
using PunimDiplome.Application.Services;        
using PunimDiplome.Infrastructure.Data;
using PunimDiplome.Infrastructure.Repositories;
using PunimDiplome.Infrastructure.Services;
using System.Security.Claims;

namespace Punim_Diplome
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var authorizedEmail = builder.Configuration["AuthorizedUser:Email"];

            
            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, sqlOptions => sqlOptions.CommandTimeout(0)));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.AddScoped<IProduktRepository, ProduktRepository>();
            builder.Services.AddScoped<IProduktService, ProduktService>();
            builder.Services.AddScoped<IImageStorage, LocalImageStorage>();
            builder.Services.AddScoped<IProduktExporter, EpplusProduktExporter>();
            builder.Services.AddScoped<IKomentRepository, KomentRepository>();
            builder.Services.AddScoped<IKomentService, KomentService>();
            builder.Services.AddScoped<IUserAdminService, UserAdminService>();



            //Injektimi i servisit per admin permision
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminEmail", policy =>
                    policy.RequireAssertion(context =>
                    {
                        var emailClaim = context.User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
                        var authorizedEmail = builder.Configuration["AuthorizedUser:Email"];
                        return emailClaim == authorizedEmail;
                    }));
            });
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;


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

            app.Run();
        }
    }
}
