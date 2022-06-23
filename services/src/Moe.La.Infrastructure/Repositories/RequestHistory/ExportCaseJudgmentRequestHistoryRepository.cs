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
    public class ExportCaseJudgmentRequestHistoryRepository : RepositoryBase, IExportCaseJudgmentRequestHistoryRepository
    {

        public ExportCaseJudgmentRequestHistoryRepository(LaDbContext commandDb, IMapper mapperConfig, IUserProvider userProvider)
            : base(commandDb, mapperConfig, userProvider)
        {
        }

        public async Task AddAsync(int id)
        {
            // get last version of data to be saved in history
            var entityInDb = db.ExportCaseJudgmentRequests.Include(d => d.Request).ThenInclude(r => r.Letter).Where(d => d.Id == id).FirstOrDefault();
            // map data to history
            var entityToAdd = mapper.Map<ExportCaseJudgmentRequestHistory>(entityInDb);

            entityToAdd.Request.CreatedBy = CurrentUser.UserId;
            entityToAdd.Request.CreatedOn = DateTime.Now;

            await db.ExportCaseJudgmentRequestHistory.AddAsync(entityToAdd);
            await db.SaveChangesAsync();
        }

        public async Task<ExportCaseJudgmentRequestHistoryListItemDto> GetAsync(int id)
        {
            var entity = await db.ExportCaseJudgmentRequestHistory
                .Include(m => m.Request)
                  .ThenInclude(mm => mm.CreatedByUser)
                .Include(m => m.Request)
                  .ThenInclude(mm => mm.Letter)
                .Where(m => m.Id == id).FirstOrDefaultAsync();

            return mapper.Map<ExportCaseJudgmentRequestHistoryListItemDto>(entity);
        }




    }
}
