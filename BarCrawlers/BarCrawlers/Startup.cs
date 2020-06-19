using BarCrawlers.Areas.Magician.Models;
using BarCrawlers.Areas.Magician.Models.Contrtacts;
using BarCrawlers.Data;
using BarCrawlers.Data.DBModels;
using BarCrawlers.Middlewares;
using BarCrawlers.Models;
using BarCrawlers.Models.Contracts;
using BarCrawlers.Services;
using BarCrawlers.Services.Contracts;
using BarCrawlers.Services.Mappers;
using BarCrawlers.Services.Mappers.Contracts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace BarCrawlers
{
    public class Startup
    {
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter((category, level) =>
                    category == DbLoggerCategory.Database.Command.Name
                    && level == LogLevel.Information)
                .AddConsole();
        });
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages()
                .AddRazorRuntimeCompilation();
            services.AddHttpClient("nominatim", n =>
                                        n.BaseAddress = new Uri("https://nominatim.openstreetmap.org/"));

            services.AddDbContext<BCcontext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("Default")).UseLoggerFactory(MyLoggerFactory));

            services.AddScoped<ICocktailsService, CocktailsService>();
            services.AddScoped<ICocktailMapper, CocktailMapper>();
            services.AddScoped<ICocktailViewMapper, CocktailViewMapper>();
            services.AddScoped<ICocktailCommentMapper, CocktailCommentMapper>();
            services.AddScoped<IIngredientsService, IngredientsService>();
            services.AddScoped<IIngredientMapper, IngredientMapper>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IUserMapper, UserMapper>();
            services.AddScoped<IUserViewMapper, UserViewMapper>();
            services.AddScoped<IBarMapper, BarMapper>();
            services.AddScoped<IBarsService, BarsService>();
            services.AddScoped<IBarViewMapper, BarViewMapper>();

            services.AddScoped<IBarUserCommentViewMapper, BarUserCommentViewMapper>();
            services.AddScoped<IBarUserCommentsService, BarUserCommentsService>();
            services.AddScoped<IBarUserCommentMapper, BarUserCommentMapper>();

            services.AddScoped<ICocktailUserCommentViewMapper, CocktailUserCommentViewMapper>();
            services.AddScoped<ICocktailCommentsService, CocktailCommentsService>();
            services.AddScoped<ICocktailCommentMapper, CocktailCommentMapper>();

            services.AddScoped<IUserInteractionsService, UserInteractionsService>();

            services.AddIdentity<User, Role>()
                .AddDefaultUI()
                .AddEntityFrameworkStores<BCcontext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.Cookie.Name = "YourAppCookieName";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = "/Identity/Account/Login";
                // ReturnUrlParameter requires 
                //using Microsoft.AspNetCore.Authentication.Cookies;
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();


            //services.AddMvc(option => option.EnableEndpointRouting = false);
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
            app.UseStaticFiles();

            app.UseRouting();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCookiePolicy();

            app.UseMiddleware<MissingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapAreaControllerRoute(
                    name: "MyAreaMagician",
                    areaName: "Magician",
                    pattern: "Magician/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                    name: "MyAreaIdentity",
                    areaName: "Identity",
                    pattern: "Identity/{controller=Home}/{action=Index}/{id?}");

                //  //app.UseRouting();
                //  app.UseEndpoints(endpoints =>
                //  {
                //      endpoints.MapControllers();
                //      endpoints.MapRazorPages();
                //      endpoints.MapControllerRoute(
                //          name: "area",
                //          pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");//{area:exists}/

                endpoints.MapRazorPages();

            });
        }
    }
}
