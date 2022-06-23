using AutoMapper;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using Moe.La.Infrastructure.DbContexts;

namespace Moe.La.Infrastructure.Repositories
{
    public abstract class RepositoryBase
    {
        protected IMapper mapper;
        private readonly IUserProvider _userProvider;
        protected LaDbContext db;

        public RepositoryBase(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider)
        {
            db = context;
            mapper = mapperConfig;
            _userProvider = userProvider;
        }

        public CurrentUser CurrentUser { get { return _userProvider.CurrentUser; } }
    }
}
