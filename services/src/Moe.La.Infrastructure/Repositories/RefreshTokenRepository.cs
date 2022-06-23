using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.Infrastructure.Repositories
{
    public class RefreshTokenRepository : RepositoryBase, IRefreshTokenRepository
    {
        public RefreshTokenRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider)
            : base(context, mapperConfig, userProvider)
        {
        }

        public async Task<ICollection<RefreshToken>> GetAllAsync()
        {
            return await db.RefreshTokens.ToListAsync();
        }
        public async Task<RefreshToken> GetAsync(int Id)
        {
            return await db.RefreshTokens.FindAsync(Id);
        }
        public async Task<RefreshTokenDto> AddAsync(RefreshTokenDto RefreshTokenDto)
        {
            var entityToAdd = mapper.Map<RefreshToken>(RefreshTokenDto);
            //entityToAdd.Id = (await commandDb.RefreshTokens.IgnoreQueryFilters().MaxAsync(c => (int?)c.Id) ?? 0) + 1;
            entityToAdd.CreatedOn = DateTime.Now;

            await db.RefreshTokens.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            return mapper.Map(entityToAdd, RefreshTokenDto);

        }

        public async Task EditAsync(RefreshTokenDto RefreshTokenDto)
        {
            var entityToUpdate = await db.RefreshTokens.FindAsync(RefreshTokenDto.Id);
            mapper.Map(RefreshTokenDto, entityToUpdate);
            await db.SaveChangesAsync();
            mapper.Map(entityToUpdate, RefreshTokenDto);

        }

        public async Task RemoveAsync(int id)
        {
            var entityToDelete = await db.RefreshTokens.FindAsync(id);
            entityToDelete.IsDeleted = true;
            await db.SaveChangesAsync();
        }

    }
}
