using BusinessObjects;
using BusinessObjects.Commons;
using BusinessObjects.Entities;
using DataAccessObjects;
using FindingHealthcareSystem.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using Repositories.Repositories;
using Services;
using Services.Interfaces;
using Services.Services;
using Services.Setups;


namespace FindingHealthcareSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddHttpClient();
            builder.Services.AddRazorPages();
            builder.Services.AddDbContext<FindingHealthcareSystemContext>(o =>
            o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));
            builder.Services.AddApplicationService();
            builder.Services.AddMemoryCache();
            builder.Services.AddSignalR();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); 
                options.Cookie.HttpOnly = true;
            });

            // Bind cấu hình từ appsettings.json
            builder.Services.Configure<CloudinarySettings>(
                builder.Configuration.GetSection("CloudinarySettings"));

/*            builder.Services.AddSingleton<CloudinaryService>();*/

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.MapHub<UpdateHub>("/updateHub");
            app.MapHub<NotificationHub>("/notificationHub");

            app.UseHttpsRedirection();
            app.UseStaticFiles();


            app.UseRouting();
            app.UseSession();

            app.UseAuthorization();
            app.MapRazorPages();


            app.Run();
        }
    }
}
