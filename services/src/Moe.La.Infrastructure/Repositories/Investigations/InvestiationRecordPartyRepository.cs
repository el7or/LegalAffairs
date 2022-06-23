using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Infrastructure.DbContexts;
using System.Threading.Tasks;

namespace Moe.La.Infrastructure.Repositories
{
    public class InvestiationRecordPartyRepository : RepositoryBase, IInvestiationRecordPartyRepository
    {
        public InvestiationRecordPartyRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider)
           : base(context, mapperConfig, userProvider)
        {
        }

        public async Task<bool> CheckPartyExist(string identityNumber, int? investigationRecordId)
        {
            var partyExist = await db.InvestigationRecordParties
                .AnyAsync(s => s.IdentityNumber == identityNumber && investigationRecordId == investigationRecordId.Value);

            return partyExist;
        }


    }
}
