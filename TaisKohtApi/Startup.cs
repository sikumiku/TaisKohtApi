using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BusinessLogic.Services;
using BusinessLogic.Factories;
using DAL.Interfaces;
using DAL.TaisKoht.EF;
using DAL.TaisKoht.EF.Helpers;
using DAL.TaisKoht.Interfaces;
using DAL.TaisKoht.Interfaces.Helpers;
using Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;


namespace TaisKohtApi
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddSingleton<IRepositoryFactory, EFRepositoryFactory>();

            services.AddScoped<IDishService, DishService>();
            services.AddScoped<IDishFactory, DishFactory>();
            services.AddScoped<IIngredientService, IngredientService>();
            services.AddScoped<IIngredientFactory, IngredientFactory>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IMenuFactory, MenuFactory>();
            services.AddScoped<IPromotionService, PromotionService>();
            services.AddScoped<IPromotionFactory, PromotionFactory>();
            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<IRestaurantFactory, RestaurantFactory>();

            services.AddScoped<IRepositoryProvider, EFRepositoryProvider>();
            services.AddScoped<IDataContext, ApplicationDbContext>();
            services.AddScoped<ITaisKohtUnitOfWork, TaisKohtEFUnitOfWork>();

            services.AddMvc();

            // Register the Swagger generator, defining one or more Swagger documents
            #region Swagger Configuration
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Täis Kõht API",
                    Description = "An ASP.NET Core API for Täis Kõht",
                    TermsOfService = "None",
                    License = new License
                    {
                        Name = "Use under LICX",
                        Url = "https://example.com/license"
                    }
                });
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
