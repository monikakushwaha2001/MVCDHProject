using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVCDHProject.Models;

namespace MVCDHProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<ICustomerDAL, CustomerSqlDALcs>();
            builder.Services.AddDbContext<MVCCoreDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConStr")));

            //for Connecting with Oracle
            //builder.Services.AddDbContext<MVCCoreDbContext>(options => options.UseOracleServer(builder.Configuration.GetConnectionString("ConStr")));

            builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<MVCCoreDbContext>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                //app.UseStatusCodePages();
                //app.UseStatusCodePagesWithRedirects("/ClientError/{0}");
                //app.UseStatusCodePagesWithReExecute("/ClientError/{0}");
                app.UseExceptionHandler("/ServerError");
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}