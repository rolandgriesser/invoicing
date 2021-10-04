using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using invoicing.server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

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

            //https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity-api-authorization?view=aspnetcore-3.0

            services.AddDefaultIdentity<Models.User>()
                .AddEntityFrameworkStores<Data.InvoicingDbContext>();

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = signingKey,
                        ValidateAudience = true,
                        ValidAudience = this.Configuration["Tokens:Audience"],
                        ValidateIssuer = true,
                        ValidIssuer = this.Configuration["Tokens:Issuer"],
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true
                    };
                });
            //     .AddOAuth();
            services.AddLogging(options => options.AddConsole());
            services.AddMvc()
                //.AddMvcOptions(options => options.Filters.Add(new AuthorizeFilter()))
                .AddNewtonsoftJson();

            services.AddSingleton<IEmailSender, EmailSender>();
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

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMvc();
        }
    }
}
