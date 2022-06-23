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
    public class NajizCaseLogRepository : RepositoryBase, INajizCaseLogRepository
    {
        public NajizCaseLogRepository(IMapper mapper, LaDbContext context, IUserProvider userProvider)
            : base(context, mapper, userProvider)
        {
        }

        public async Task AddAsync(NajizCaseDto najizCaseDto)
        {
            var entityToAdd = mapper.Map<NajizCase>(najizCaseDto);

            entityToAdd.CreatedBy = ApplicationConstants.SystemAdministratorId;

            foreach (var party in entityToAdd.Parties)
            {
                party.CreatedBy = ApplicationConstants.SystemAdministratorId;
            }

            foreach (var hearing in entityToAdd.Hearings)
            {
                hearing.CreatedBy = ApplicationConstants.SystemAdministratorId;
            }

            await db.NajizCases.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            mapper.Map(entityToAdd, najizCaseDto);
        }

    }
}
