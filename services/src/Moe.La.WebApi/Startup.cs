using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Moe.La.Core.Constants;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Logging;
using Moe.La.Infrastructure.DbContexts;
using Moe.La.Integration.Options;
using Moe.La.ServiceInterface;
using Moe.La.ServiceInterface.Auth;
using Moe.La.WebApi.Filters;
using Moe.La.WebApi.Middlewares;
using Moe.La.WebApi.Options;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Linq;
using System.Text;

namespace Moe.La.WebApi
{
    public class Startup
    {
        private readonly string LaCorsPolicy = "LaCorsPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // very important to includes input and output formatters for JSON (such as Enum)
            // Install - Package Microsoft.AspNetCore.Mvc.NewtonsoftJson - Version 3.1.3
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(DbContextTransactionFilter));
                options.Filters.Add(new AuthorizeFilter());
            }).AddNewtonsoftJson(options =>
            {
                // JsonSerializationException: Self referencing loop
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            // supported encoding name for PDF ==> Install-Package System.Text.Encoding
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            //    AddDbContext ==> Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore -Version 3.1.3
            //    UseSqlServer ==> Install - Package Microsoft.EntityFrameworkCore.SqlServer - Version 3.1.3
            services.AddDbContext<LaDbContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:Default"],
                    sqlOption =>
                    {
                        //sqlOption.EnableRetryOnFailure();
                        sqlOption.MigrationsAssembly("Moe.La.Infrastructure");
                    });
            });

            services.AddDatabaseDeveloperPageExceptionFilter();

            //  Add AutoMapper ==> 
            //  Install - Package AutoMapper - Version 10.0.0
            //  Install - Package AutoMapper.Extensions.Microsoft.DependencyInjection - Version 8.0.1
            services.AddAutoMapper();

            services.AddCors(options =>
            {
                options.AddPolicy(LaCorsPolicy, policy =>
                {
                    policy.WithOrigins(Configuration.GetSection("Clients").Get<ClientConfiguration[]>().SelectMany(m => m.Urls).ToArray());
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                });
            });

            services.AddLocalization();

            // Get the configrations from appsettings.json
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<PhotoSettings>(Configuration.GetSection("PhotoSettings"));
            services.Configure<FileSettings>(Configuration.GetSection("FileSettings"));

            // Configure application services.
            services.AddServices();

            // Configure repositories.
            services.AddRepositories();

            // Configure localization resources.
            services.AddLocalizationResources();

            // Configure external service integration.
            services.AddIntegrationServices();

            // Configure Redis connection.
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("Redis");
                options.InstanceName = "LAS_";
            });

            // adds the default identity system configuration.
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

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "LAS API", Version = "v1" });
            });

            // Configure MOE email service options.
            services.Configure<MoeEmailOptions>(Configuration.GetSection("Integration:MoeEmailService"));

            // Configure MOE SMS service options.
            services.Configure<MoeSmsOptions>(Configuration.GetSection("Integration:MoeSmsService"));

            // Instead of specifying roles in the [Authorize] attribute, specify the policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("LitigationManagerOrRegionsSupervisor", authBuilder =>
                authBuilder.RequireAssertion(context => (context.User.IsInRole(ApplicationRolesConstants.DepartmentManager.Name) && context.User.HasClaim(ApplicationRolesConstants.DepartmentManager.Name, Convert.ToString((int)Departments.Litigation)))
                        || context.User.IsInRole(ApplicationRolesConstants.RegionsSupervisor.Name)));

                options.AddPolicy("LitigationManagerOrBranchManager", authBuilder =>
                authBuilder.RequireAssertion(context => (context.User.IsInRole(ApplicationRolesConstants.DepartmentManager.Name) && context.User.HasClaim(ApplicationRolesConstants.DepartmentManager.Name, Convert.ToString((int)Departments.Litigation)))
                        || context.User.IsInRole(ApplicationRolesConstants.BranchManager.Name)));

                options.AddPolicy("LitigationManager", authBuilder =>
                authBuilder.RequireAssertion(context => context.User.IsInRole(ApplicationRolesConstants.DepartmentManager.Name) && context.User.HasClaim(ApplicationRolesConstants.DepartmentManager.Name, Convert.ToString((int)Departments.Litigation))));

                options.AddPolicy(ApplicationRolesConstants.LegalResearcher.Name, authBuilder =>
                authBuilder.RequireRole(ApplicationRolesConstants.LegalResearcher.Name));

                options.AddPolicy(ApplicationRolesConstants.Investigator.Name, authBuilder =>
                authBuilder.RequireAssertion(context => context.User.IsInRole(ApplicationRolesConstants.GeneralSupervisor.Name)
                || (context.User.IsInRole(ApplicationRolesConstants.DepartmentManager.Name) && context.User.HasClaim(ApplicationRolesConstants.DepartmentManager.Name, Convert.ToString((int)Departments.Investigation)))
                || context.User.IsInRole(ApplicationRolesConstants.Investigator.Name)));

                options.AddPolicy("Moamalat", authBuilder =>
                authBuilder.RequireAssertion(context => context.User.IsInRole(ApplicationRolesConstants.GeneralSupervisor.Name)
                || context.User.IsInRole(ApplicationRolesConstants.Distributor.Name)));

                options.AddPolicy("MoamlatConfedential", authBuilder =>
                authBuilder.RequireAssertion(context => context.User.IsInRole(ApplicationRolesConstants.GeneralSupervisor.Name)
                || (context.User.IsInRole(ApplicationRolesConstants.Distributor.Name) && context.User.HasClaim("Permission", "ConfidentialMoamla"))));
            });

            services.AddScoped<IJwtFactory, JwtFactory>();

            // jwt wire up
            // Get options from app settings
            var JwtIssuerOptions = Configuration.GetSection("JwtIssuerOptions");

            // key generated using symmetric algorithms
            var _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtIssuerOptions.GetValue<string>("Secret")));

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

            // Access to the HttpContext inside a service.
            services.AddHttpContextAccessor();

            // Register the current logged in user service.
            services.AddTransient<IUserProvider, CurrentUserProvider>();

            services.AddScoped<IClaimsTransformation, ClaimsTransformer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();

                var swaggerOptions = new SwaggerOptions();
                Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

                app.UseSwagger(options =>
                {
                    options.RouteTemplate = swaggerOptions.JsonRoute;
                });

                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description);
                });
            }
            //else
            //{
            //    app.UseExceptionHandler("/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    //app.UseHsts();
            //}

            ApplicationLogger.ConfigureLogger(loggerFactory);

            app.UseStaticFiles();

            // Use custom middleware to configure http security headers.
            app.UseMiddleware<SecurityHeadersMiddleware>();

            //app.UseHttpsRedirection();

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseCors(LaCorsPolicy);

            // Put here (after UseRouting & before  UseEndpoints)
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                if (env.IsDevelopment())
                {
                    endpoints.MapGet("/show-config", async context =>
                    {
                        var configInfo = (Configuration as IConfigurationRoot).GetDebugView();
                        await context.Response.WriteAsync(configInfo);
                    });
                }

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}").RequireAuthorization();
            });
        }
    }

    public class ClientConfiguration
    {
        public string Name { get; set; }
        public string[] Urls { get; set; }
    }
}