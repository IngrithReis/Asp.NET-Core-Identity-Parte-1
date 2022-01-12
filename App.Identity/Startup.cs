using App.Identity.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace App.Identity
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            var migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<UsersDbContext>(
                opt => opt.UseSqlServer(@""
                                        , sql => sql.MigrationsAssembly(migrationAssembly)));



            services.AddIdentity<Users, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;

                // configuração Password (dados requeridos em criação de senha, ou não requeridos, tamanho etc...)

                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;

                // opçãoes para bloqueio de usuário em tentativas de vezes de login
                options.Lockout.MaxFailedAccessAttempts = 3;

                options.Lockout.AllowedForNewUsers = true;




            })
                    .AddEntityFrameworkStores<UsersDbContext>()
                    .AddDefaultTokenProviders()


            // implementação da classe "NaoContemValidadorSenha"
                    .AddPasswordValidator<NaoContemValidadorSenha<Users>>();

            services.Configure<DataProtectionTokenProviderOptions>(
                options => options.TokenLifespan = TimeSpan.FromHours(3));

            services.AddScoped<IUserClaimsPrincipalFactory<Users>, UsersClaimPrincipalFactory>();
            services.ConfigureApplicationCookie(options => options.LoginPath = "/Home/Login");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
