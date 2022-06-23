using AutoMapper;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Infrastructure.DbContexts;
using System;
using System.Threading.Tasks;

namespace Moe.La.Infrastructure.Repositories
{
    public class MoamalaTransactionRepository : RepositoryBase, IMoamalaTransactionRepository
    {

        public MoamalaTransactionRepository(LaDbContext commandDb, IMapper mapperConfig, IUserProvider userProvider)
            : base(commandDb, mapperConfig, userProvider)
        {
        }

        public async Task AddAsync(MoamalaTransactionDto moamalaTransactionDto)
        {
            var entityToAdd = mapper.Map<MoamalaTransaction>(moamalaTransactionDto);
            entityToAdd.CreatedBy = CurrentUser.UserId;
            entityToAdd.CreatedOn = DateTime.Now;

            await db.MoamalaTransactions.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            mapper.Map(entityToAdd, moamalaTransactionDto);
        }



    }
}
