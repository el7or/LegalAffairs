using AutoMapper;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using Moe.La.Infrastructure.DbContexts;
using Moe.La.Integration.Options;
using Moe.La.Job.Filters;
using Moe.La.ServiceInterface;
using Moe.La.ServiceInterface.Auth;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Text;

namespace Moe.La.Job
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
            // Access to the HttpContext inside a service.
            services.AddHttpContextAccessor();

            // Register the current logged in user service.
            services.AddTransient<IUserProvider, CurrentUserProvider>();

            // very important to includes input and output formatters for JSON (such as Enum)
            // Install - Package Microsoft.AspNetCore.Mvc.NewtonsoftJson - Version 3.1.3
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
            {
                // JsonSerializationException: Self referencing loop
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            // AddDbContext ==> Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore -Version 3.1.3
            // UseSqlServer ==> Install - Package Microsoft.EntityFrameworkCore.SqlServer - Version 3.1.3
            services.AddDbContext<LaDbContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:Default"],
                    sqlOption =>
                    {
                        //sqlOption.EnableRetryOnFailure();
                        sqlOption.MigrationsAssembly("Moe.La.Infrastructure");
                    });
            });

            // adds the default identity system configuration
            var builder = services.AddIdentity<AppUser, AppRole>(options =>
            {
                // configure identity options
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            });

            builder.AddEntityFrameworkStores<LaDbContext>().AddDefaultTokenProviders();

            //  Add AutoMapper ==> 
            //  Install - Package AutoMapper - Version 10.0.0
            //  Install - Package AutoMapper.Extensions.Microsoft.DependencyInjection - Version 8.0.1
            services.AddAutoMapper();

            services.AddSingleton(new HangfireConfigModel
            {
                HourlyIntervalCron = Configuration.GetSection("HourlyIntervalCron").Value,
                DailyIntervalCorn = Configuration.GetSection("DailyIntervalCorn").Value,
                MonthlyIntervalCorn = Configuration.GetSection("MonthlyIntervalCorn").Value

            });

            // Configure Redis connection.
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("Redis");
                options.InstanceName = "LAS_";
            });

            services.AddHostedService<RecurringJobsService>();

            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("Default"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            // Add ASP.Net Core Session State.
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);//You can set Time   
                options.Cookie = new CookieBuilder() { Name = "LegalAffairs.Jobs.LocalAuth", HttpOnly = true };
            });

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            // Configure repositories.
            services.AddRepositories();

            // Configure localization resources.
            services.AddLocalizationResources();

            services.AddLocalization();

            // Add application services.
            services.AddServices();

            // Add application repositories.
            services.AddRepositories();

            JwtIssuerOptions(services);

            // Configure MOE email service options.
            services.Configure<MoeEmailOptions>(Configuration.GetSection("Integration:MoeEmailService"));

            // Configure MOE SMS service options.
            services.Configure<MoeSmsOptions>(Configuration.GetSection("Integration:MoeSmsService"));
        }

        private void JwtIssuerOptions(IServiceCollection services)
        {
            services.AddScoped<IJwtFactory, JwtFactory>();


            // jwt wire up
            // Get options from app settings
            var JwtIssuerOptions = Configuration.GetSection("JwtIssuerOptions");

            // key generated using symmetric algorithms
            var _signingKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(JwtIssuerOptions.GetValue<string>("Secret")));

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = JwtIssuerOptions.GetValue<string>("Issuer");
                options.Audience = JwtIssuerOptions.GetValue<string>("Audience");
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            // Add Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = JwtIssuerOptions.GetValue<string>("Issuer");
                configureOptions.SaveToken = true;
                configureOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    IssuerSigningKey = _signingKey,
                    ValidIssuer = JwtIssuerOptions.GetValue<string>("Issuer"),
                    ValidateAudience = true,
                    ValidAudience = JwtIssuerOptions.GetValue<string>("Audience"),
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IBackgroundJobClient backgroundJobs, IWebHostEnvironment env)
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
            app.UseSession();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthorization();

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangFireAuthFilter() }
            });

            //app.UseHangfireDashboard("/jobs");

            //backgroundJobs.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
                endpoints.MapHangfireDashboard();
            });
        }
    }
}
