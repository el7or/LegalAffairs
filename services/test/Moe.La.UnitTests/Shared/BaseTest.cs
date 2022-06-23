using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Mapping;
using Moe.La.Core.Models;
using Moe.La.Infrastructure;
using Moe.La.Infrastructure.DbContexts;
using Moq;

namespace Moe.La.UnitTests
{
    public abstract class BaseUnitTest
    {
        protected LaDbContext Db { get; private set; }

        protected ServiceHelper ServiceHelper { get; private set; }

        private static UserManager<AppUser> _userManager;

        private static RoleManager<AppRole> _roleManager;

        public IDistributedCache _cache { get; private set; }

        private static string _dbName { get; set; }

        //private static readonly InMemoryDatabaseRoot InMemoryDatabaseRoot = new InMemoryDatabaseRoot();

        public BaseUnitTest(string dbName = "moe-la")
        {
            _dbName = dbName;
            // Setup the required services.
            var options = Setup();

            // Create db context.
            Db = new LaDbContext(options);

            //Setup();

            // Initialize db.
            SeedData.Initialize(Db, true);

            // Initialize service helper.
            InitializeServiceHelper();
        }

        private static DbContextOptions<LaDbContext> Setup()
        {
            var services = new ServiceCollection();

            services
                .AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<DbContext>((sp, options) =>
                {
                    options.UseInMemoryDatabase(_dbName);
                    options.UseInternalServiceProvider(sp);
                }).AddLogging();

            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            })
            .AddUserManager<UserManager<AppUser>>()
            .AddEntityFrameworkStores<DbContext>()
            .AddDefaultTokenProviders();

            var serviceProvider = services.BuildServiceProvider();
            _userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            _roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();

            var builder = new DbContextOptionsBuilder<LaDbContext>()
                .UseInMemoryDatabase(_dbName)
                .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        private void InitializeServiceHelper()
        {
            var mapper = new MapperConfiguration(m => m.AddMaps(typeof(TransactionMappingProfile).Assembly)).CreateMapper();

            //var httpContext = new DefaultHttpContext()
            //{
            //    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            //    {
            //        new Claim("id", TestUsers.AdminId.ToString()),
            //    }, JwtBearerDefaults.AuthenticationScheme))
            //};
            //var httpContextAccessor = new Mock<IHttpContextAccessor>();
            //httpContextAccessor.Setup(m => m.HttpContext).Returns(httpContext);

            var userProvider = new Mock<IUserProvider>();

            var currentUser = new CurrentUser
            {
                UserId = TestUsers.AdminId
            };

            userProvider.Setup(m => m.CurrentUser).Returns(currentUser);

            ServiceHelper = new ServiceHelper(Db, mapper, userProvider.Object, _userManager, _roleManager, _cache);
        }
    }
}
