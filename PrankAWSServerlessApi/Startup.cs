using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NgrokAspNetCore;
using Prank.DAL;
using PrankAWSServerlessApi.ApiSettings;
using PrankAWSServerlessApi.Infrastructure;

namespace PrankAWSServerlessApi
{
    public class Startup
    {
        public const string AppS3BucketKey = "AppS3Bucket";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public static IConfiguration Configuration { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
          
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
            });

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.ClaimsIssuer = Configuration["Jwt:Issuer"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
              
            });
         

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

            Configuration.GetSection("AppSettings").Bind(AppSettings.Default);

            services.AddMvc().AddSessionStateTempDataProvider()
                           .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache

            services.AddSession(options =>
            {
                options.Cookie.Name = ".Project.Session";
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddTransient<DataContext>(s => new DataContext(connectionString));

            var emailSection = Configuration.GetSection("EmailSettings");
            var emailSettings = emailSection.Get<EmailSettings>();
            services.AddSingleton(typeof(EmailSettings), emailSettings);

            var filesSection = Configuration.GetSection("FilesSettings");
            var fileSettings = filesSection.Get<FileSettings>();
            services.AddSingleton(typeof(FileSettings), fileSettings);

            var twilioSection = Configuration.GetSection("TwilioSettings");
            var twilioSettings = twilioSection.Get<TwilioSettings>();
            services.AddSingleton(typeof(TwilioSettings), twilioSettings);

            var awsSection = Configuration.GetSection("AWSSettings");
            var awsSettings = awsSection.Get<AWSSettings>();
            services.AddSingleton(typeof(AWSSettings), awsSettings);

            services.AddControllers();


            services.AddNgrok();
            // Add S3 to the ASP.NET Core dependency injection framework.
            services.AddAWSService<Amazon.S3.IAmazonS3>();


        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Main/Error");
            }

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseSession();

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    //options.SwaggerEndpoint("./swagger/v1/swagger.json", "MyAPI V1");
                    options.SwaggerEndpoint("./swagger/v1/swagger.json", "MyAPI V1");
                    options.RoutePrefix = string.Empty;
                });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                  );
            });



        }
    }   
}
