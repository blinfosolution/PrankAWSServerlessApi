using System;
using Amazon.S3;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PrankAWSWebApp.ApiSettings;
using PrankAWSWebApp.Areas.Admin.Data;
using PrankAWSWebApp.AwsS3;
using PrankAWSWebApp.Models;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using NgrokAspNetCore;

namespace PrankAWSWebApp
{
    public class Startup
    {
        private IConfiguration config;
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(env.ContentRootPath);
            builder.AddJsonFile("appsettings.json");
            config = builder.Build();

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings  
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(10);
                options.LoginPath = "/Admin/Account/Login";
                options.AccessDeniedPath = "/Admin/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            services.AddDbContext<PrankAWSIdentityDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            services.AddIdentity<PrankIdentityUser, PrankIdentityRole>()
                .AddEntityFrameworkStores<PrankAWSIdentityDbContext>();

           services.AddMvc();
            services.Configure<MySettingsModel>(Configuration.GetSection("MySettings"));

            var emailSection = Configuration.GetSection("EmailSettings");
            var emailSettings = emailSection.Get<EmailSettings>();
            services.AddSingleton(typeof(EmailSettings), emailSettings);

            

            var twilioSection = Configuration.GetSection("TwilioSettings");
            var twilioSettings = twilioSection.Get<TwilioSettings>();
            services.AddSingleton(typeof(TwilioSettings), twilioSettings);



            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddAWSService<IAmazonS3>();
            services.AddSingleton<IAwsS3HelperService, AwsS3HelperService>();
            services.Configure<AwsS3BucketOptions>(Configuration.GetSection(nameof(AwsS3BucketOptions)))
                .AddSingleton(x => x.GetRequiredService<IOptions<AwsS3BucketOptions>>().Value);

            services.AddTransient<CustomClaimsCookieSignInHelper<PrankIdentityUser>>();
            services.AddNgrok();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                 name: "default",
                 pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                 name: "areas", "Admin",
                 pattern: "{area:exists}/{controller=Home}/{action=Index}/{Id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
