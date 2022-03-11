using System;
using ArchimydesWeb.Data;
using ArchimydesWeb.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(ArchimydesWeb.Areas.Identity.IdentityHostingStartup))]
namespace ArchimydesWeb.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<ArchimydesWebContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("ArchimydesWebContextConnection")));

                services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<ApplicationRole>()
                    .AddEntityFrameworkStores<ArchimydesWebContext>();
            });
        }
    }
}