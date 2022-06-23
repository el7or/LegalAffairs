using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Infrastructure.DbContexts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Moe.La.Infrastructure.Repositories
{
    public class CaseSupportingDocumentRequestHistoryRepository : RepositoryBase, ICaseSupportingDocumentRequestHistoryRepository
    {

        public CaseSupportingDocumentRequestHistoryRepository(LaDbContext commandDb, IMapper mapperConfig, IUserProvider userProvider)
            : base(commandDb, mapperConfig, userProvider)
        {
        }

        public async Task AddAsync(int id)
        {
            // get last version of data to be saved in history
            var entityInDb = db.DocumentRequests.Include(d => d.Request).ThenInclude(r => r.Letter).Include(d => d.Documents).Where(d => d.Id == id).FirstOrDefault();
            // map data to history
            var entityToAdd = mapper.Map<CaseSupportingDocumentRequestHistory>(entityInDb);

            entityToAdd.Request.CreatedBy = CurrentUser.UserId;
            entityToAdd.Request.CreatedOn = DateTime.Now;

            await db.CaseSupportingDocumentRequestHistory.AddAsync(entityToAdd);
            await db.SaveChangesAsync();
        }

        public async Task<CaseSupportingDocumentRequestHistoryListItemDto> GetAsync(int id)
        {
            var entity = await db.CaseSupportingDocumentRequestHistory
                .Include(m => m.Request)
                  .ThenInclude(mm => mm.Letter)
                .Include(m => m.Request)
                  .ThenInclude(mm => mm.CreatedByUser)
                .Include(m => m.Documents)
                .Include(m => m.ConsigneeDepartment)
                .Where(m => m.Id == id).FirstOrDefaultAsync();

            return mapper.Map<CaseSupportingDocumentRequestHistoryListItemDto>(entity);
        }


    }
}
