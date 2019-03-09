using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace invoicing.server
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
            // services.Configure<CookiePolicyOptions>(options =>
            //     {
            //         options.CheckConsentNeeded = context => true;
            //         options.MinimumSameSitePolicy = SameSiteMode.None;
            //     });



            //     services.Configure<IdentityOptions>(options =>
            //     {
            //         // Password settings.
            //         options.Password.RequireDigit = true;
            //         options.Password.RequireLowercase = true;
            //         options.Password.RequireNonAlphanumeric = true;
            //         options.Password.RequireUppercase = true;
            //         options.Password.RequiredLength = 6;
            //         options.Password.RequiredUniqueChars = 1;

            //         // Lockout settings.
            //         options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            //         options.Lockout.MaxFailedAccessAttempts = 5;
            //         options.Lockout.AllowedForNewUsers = true;

            //         // User settings.
            //         options.User.AllowedUserNameCharacters =
            //         "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            //         options.User.RequireUniqueEmail = false;
            //     });

            //     services.ConfigureApplicationCookie(options =>
            //     {
            //         // Cookie settings
            //         options.Cookie.HttpOnly = true;
            //         options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

            //         options.LoginPath = "/Identity/Account/Login";
            //         options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            //         options.SlidingExpiration = true;
            //     });
            // services.Configure<CookiePolicyOptions>(options =>
            // {
            //     options.CheckConsentNeeded = context => true;
            //     options.MinimumSameSitePolicy = SameSiteMode.None;
            // });

            services.AddDbContext<Data.InvoicingDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("InvoicingDbContextConnection"))
            );

            services.AddDefaultIdentity<Models.User>()
                .AddUserStore<Data.InvoicingDbContext>();

            services.AddLogging(options => options.AddConsole());
            services.AddMvc()
                //.AddMvcOptions(options => options.Filters.Add(new AuthorizeFilter()))
                .AddNewtonsoftJson();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting(routes =>
            {
                routes.MapControllers()
                    .RequireAuthorization();
            });

            app.UseAuthorization();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
