using DentistRegistrationFormData;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace DentistRegistrationForm
{
    public class Startup
    {
        enum DbProviders
        {
            SqlServer = 1,
            MySql = 2
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            var dbNumber = (DbProviders)Configuration.GetValue<int>("Application:DatabaseProvider");

            services.AddDbContext<AppDbContext>(options =>
            {
                switch (dbNumber)
                {
                    case DbProviders.SqlServer:
                        options.UseSqlServer(
                            Configuration.GetConnectionString("SqlServer"),
                            config =>
                            {
                                config.MigrationsAssembly("MigrationsSqlServer");
                            });
                        break;
                    case DbProviders.MySql:
                        options.UseMySql(
                            Configuration.GetConnectionString("MySql"),
                            ServerVersion.AutoDetect(Configuration.GetConnectionString("MySql")),
                            config =>
                            {
                                config.MigrationsAssembly("MigrationsMySql");
                            });
                        break;
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            context.Database.Migrate();
        }
    }
}
