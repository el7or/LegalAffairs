using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Infrastructure.DbContexts;
using Moe.La.Infrastructure.Extensions;
using System.Linq;
using System.Threading.Tasks;

namespace Moe.La.Infrastructure.Repositories
{
    public class RoleClaimRepository : RepositoryBase, IRoleClaimRepository
    {
        //private readonly RoleManager<AppRole> _roleManager;

        public RoleClaimRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider)
            : base(context, mapperConfig, userProvider)
        {
            //_roleManager = roleManager;
        }

        public async Task<QueryResultDto<RoleClaimListItemDto>> GetAllAsync(QueryObject queryObject)
        {
            var result = new QueryResult<AppRoleClaim>();

            //var role = _roleManager.Roles.Single(x => x.Id == roleClaimQueryObject.RoleId);
            //var query = _roleManager.GetClaimsAsync(role).Result
            //     .AsQueryable();

            var query = db.RoleClaims
                .AsNoTracking()
                .AsQueryable();


            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObject);

            result.Items = await query.ToListAsync();

            return mapper.Map<QueryResult<AppRoleClaim>, QueryResultDto<RoleClaimListItemDto>>(result);
        }

    }
}
