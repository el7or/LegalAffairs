using AutoMapper;
using Moe.La.Core.Constants;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Infrastructure.DbContexts;
using System.Threading.Tasks;

namespace Moe.La.Infrastructure.Repositories
{
    public class MoeenCaseLogRepository : RepositoryBase, IMoeenCaseLogRepository
    {
        public MoeenCaseLogRepository(IMapper mapper, LaDbContext context, IUserProvider userProvider)
            : base(context, mapper, userProvider)
        {
        }

        public async Task AddAsync(MoeenCaseDto moeenCaseDto)
        {
            var entityToAdd = mapper.Map<MoeenCase>(moeenCaseDto);

            entityToAdd.CreatedBy = ApplicationConstants.SystemAdministratorId;

            foreach (var party in entityToAdd.Parties)
            {
                party.CreatedBy = ApplicationConstants.SystemAdministratorId;
            }

            foreach (var hearing in entityToAdd.Hearings)
            {
                hearing.CreatedBy = ApplicationConstants.SystemAdministratorId;
            }

            await db.MoeenCases.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            mapper.Map(entityToAdd, moeenCaseDto);
        }

    }
}
