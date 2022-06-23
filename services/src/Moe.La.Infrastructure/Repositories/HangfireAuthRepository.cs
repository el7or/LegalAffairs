using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moe.La.Core.Constants;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Infrastructure.DbContexts;
using System.Linq;
using System.Threading.Tasks;

namespace Moe.La.Infrastructure.Repositories
{
    public class HangfireAuthRepository : RepositoryBase, IHangfireAuthRepository
    {

        public HangfireAuthRepository(LaDbContext context, IMapper mapper, IUserProvider userProvider)
            : base(context, mapper, userProvider)
        {
        }

        public async Task<AppUser> GetAdminByUserName(string userName)
        {
            var user = await db.Users
                .Include(m => m.UserRoles)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserName == userName && u.UserRoles.Any(ur => ur.RoleId == ApplicationRolesConstants.Admin.Code));

            return mapper.Map<AppUser>(user);
        }
    }
}