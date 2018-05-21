using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Services;
using BusinessLogic.Factories;
using DAL.Interfaces;
using DAL.TaisKoht.EF;
using DAL.TaisKoht.EF.Helpers;
using DAL.TaisKoht.Interfaces;
using DAL.TaisKoht.Interfaces.Helpers;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Http;
using React.AspNet;
using Microsoft.AspNetCore.SpaServices.Webpack;



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
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddCookie(options => { options.SlidingExpiration = true; })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration["Token:Issuer"],
                        ValidAudience = Configuration["Token:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["Token:Key"])
                        )
                    };

                    #region JwtToken Life Cycle
                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = async (context) =>
                        {
                            var userManager = context.HttpContext.RequestServices.GetService<UserManager<User>>();
                            var user = await userManager.FindByEmailAsync(context.Principal.Identity.Name);
                            if (user == null || user.LockoutEnd > DateTime.Now)
                            {
                                context.Response.StatusCode = 401;
                            }
                        }
                    };
                    #endregion

                });

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
            services.AddScoped<IUserFactory, UserFactory>();

            services.AddScoped<IRepositoryProvider, EFRepositoryProvider>();
            services.AddScoped<IDataContext, ApplicationDbContext>();
            services.AddScoped<ITaisKohtUnitOfWork, TaisKohtEFUnitOfWork>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddReact();

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
            return services.BuildServiceProvider();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services)
        {
            #region Error Handler registration
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            #endregion

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TäisKõht API V1");
            });

            #region React App
            // Initialise ReactJS.NET. Must be before static files.
            app.UseReact(config =>
            {
                // If you want to use server-side rendering of React components,
                // add all the necessary JavaScript files here. This includes
                // your components as well as all of their dependencies.
                // See http://reactjs.net/ for more information. Example:
                //config
                //  .AddScript("~/Scripts/First.jsx")
                //  .AddScript("~/Scripts/Second.jsx");

                // If you use an external build too (for example, Babel, Webpack,
                // Browserify or Gulp), you can improve performance by disabling
                // ReactJS.NET's version of Babel and loading the pre-transpiled
                // scripts. Example:
                //config
                //  .SetLoadBabel(false)
                //  .AddScriptWithoutTransform("~/Scripts/bundle.server.js");
            });
            #endregion

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc();

            CreateUserRoles(services).Wait();
        }

        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<User>>();

            var roleCheck1 = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck1)
            {
                await RoleManager.CreateAsync(new IdentityRole("Admin"));
            }
            var roleCheck2 = await RoleManager.RoleExistsAsync("normalUser");
            if (!roleCheck2)
            {
                await RoleManager.CreateAsync(new IdentityRole("normalUser"));
            }
            var roleCheck3 = await RoleManager.RoleExistsAsync("premiumUser");
            if (!roleCheck3)
            {
                await RoleManager.CreateAsync(new IdentityRole("premiumUser"));
            }
            var admin = new User { UserName = "admin", Email = "admin@gmail.com" };
            var result = await UserManager.FindByEmailAsync(admin.Email);
            if (result == null)
            {
                await UserManager.CreateAsync(admin, "Aa123456789.");
                var currentUser = await UserManager.FindByEmailAsync(admin.Email);
                await UserManager.AddToRoleAsync(currentUser, "Admin");
            }
        }
    }
}
