using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Services;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace SalesWebMvc
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<SalesWebMvcContext>(options =>
                    options.UseMySql(Configuration.GetConnectionString("SalesWebMvcContext"), Builder =>//Adicionando o provider do MySql para as migrations
                        Builder.MigrationsAssembly("SalesWebMvc")));
            
            services.AddScoped<SeedingService>(); //Adicionando SeedingService para registrar no sistema de injeção de depência da aplicação em outras classes
            services.AddScoped<SellerService>(); //Adicionando SellerService para registrar no sistema de injeção de dependência da aplicação em outras classes
            services.AddScoped<DepartmentService>();//Agora este serviço pode ser injetado pelo mecanismo de injeção de dependencias
            services.AddScoped<SalesRecordService>();//Agora este serviço pode ser injetado pelo mecanismo de injeção de dependencias
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, SeedingService seedingService) //Incluindo o serviço nos parâmetros para poder chamar o método seed()
        {
            //Bloco que configura o locale da aplicação com sendo os Estados Unidos
            var enUs = new CultureInfo("en-Us"); 
            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(enUs),
                SupportedCultures = new List<CultureInfo> { enUs },
                SupportedUICultures = new List<CultureInfo> { enUs }
            };

            app.UseRequestLocalization(localizationOptions);
            // Fim do bloco de configuração do locale da aplicação

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                seedingService.Seed(); //chamando o serviço de popular base de dados
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
