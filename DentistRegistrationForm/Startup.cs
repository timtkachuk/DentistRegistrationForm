using DentistRegistrationForm.Services;
using DentistRegistrationFormData;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVCCoreEStore.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
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
                options.UseLazyLoadingProxies();

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

            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = Configuration.GetValue<bool>("Security:PasswordPolicy:RequireDigit");
                options.Password.RequiredLength = Configuration.GetValue<int>("Security:PasswordPolicy:RequiredLength");
                options.Password.RequireLowercase = Configuration.GetValue<bool>("Security:PasswordPolicy:RequireLowercase");
                options.Password.RequireNonAlphanumeric = Configuration.GetValue<bool>("Security:PasswordPolicy:RequireNonAlphanumeric");
                options.Password.RequireUppercase = Configuration.GetValue<bool>("Security:PasswordPolicy:RequireUppercase");

                options.SignIn.RequireConfirmedEmail = Configuration.GetValue<bool>("Security:PasswordPolicy:RequireConfirmedEmail");

            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider);

            services.AddTransient<IMailMessageService, MailMessageService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            AppDbContext context,
            RoleManager<Role> roleManager,
            UserManager<User> userManager
            )
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

            app.UseAuthentication();

            app.UseAuthorization();

            var cultures = new List<CultureInfo> {
                                new CultureInfo("ro-MD")
                            };
            app.UseRequestLocalization(options =>
            {
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("ro-MD");
                options.SupportedCultures = cultures;
                options.SupportedUICultures = cultures;
            });

            app.UseEndpoints(endpoints =>
            {
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                      name: "areas",
                      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                    );
                });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            context.Database.Migrate();

            new[]
            {
                new Role { Name = "Administrators"},
                new Role { Name = "Doctors"},
                new Role { Name = "Clients"}
            }
            .ToList()
            .ForEach(role =>
            {
                roleManager.CreateAsync(role).Wait();
            });

            {
                var user = new User
                {
                    UserName = Configuration.GetValue<string>("Security:DefaultUser:UserName"),
                    Name = Configuration.GetValue<string>("Security:DefaultUser:Name")
                };
                user.EmailConfirmed = true;
                userManager.CreateAsync(user, Configuration.GetValue<string>("Security:DefaultUser:Password")).Wait();
                userManager.AddToRoleAsync(user, "Administrators").Wait();
            }
        }
    }
}
