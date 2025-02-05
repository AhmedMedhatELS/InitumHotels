using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Models;
using DataAccess.Repository.IRepository;
using DataAccess.Repository;
using Shared;
using Microsoft.AspNetCore.Identity.UI.Services;
using Utility;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using InitumHotels.Hubs;

namespace InitumHotels
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.AddSignalR();

            #region DataBase
            builder.Services.AddDbContext<ApplicationDbContext>(options
                => options.UseSqlServer(builder.Configuration
                .GetConnectionString("DefaultConnection")));
            #endregion
            #region Identity
            builder.Services.AddDefaultIdentity<ApplicationUser>(options
                => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            #endregion
            #region Email Sender
            builder.Services.AddScoped<IEmailSender, EmailSender>();
            #endregion
            #region Register With
            builder.Services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId =
                "909079649326-v1bcogq9o8vh9ag8eu6cd68jkcmoi35p.apps.googleusercontent.com";
                googleOptions.ClientSecret =
                "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";
            });
            #endregion
            #region Unit of Work
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<UnitOfWork>();
            #endregion
            #region Base Services
            builder.Services.AddScoped<BaseService>();
            builder.Services.AddScoped<RoomHelper>();
            builder.Services.AddScoped<HotelHelper>();
            builder.Services.AddScoped<ReservationHelper>();
            #endregion
            var app = builder.Build();

            #region Inteal Create Roles And 1st Admin

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await IntailRolesAdmin.SeedRolesAsync(services);
                await IntailRolesAdmin.SeedAdminUserAsync(services);
            }
            #endregion

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();

            #region Map SignalR hub route

            app.MapHub<RoomHub>("/roomHub");

            #endregion

            app.MapRazorPages();
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
