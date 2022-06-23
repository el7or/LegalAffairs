using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Infrastructure.DbContexts;
using Moe.La.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Moe.La.Infrastructure.Repositories
{
    public class PartyRepository : RepositoryBase, IPartyRepository
    {
        public PartyRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider)
            : base(context, mapperConfig, userProvider)
        {
        }

        public async Task<QueryResultDto<PartyListItemDto>> GetAllAsync(PartyQueryObject queryObject)
        {
            var result = new QueryResult<Party>();

            var query = db.Parties
                .Include(a => a.IdentityType)
                .Include(s => s.City)
                .ThenInclude(p => p.Province)
                .OrderBy(n => n.Name)
                .AsNoTracking()
                .AsQueryable();

            if (queryObject.CityId.HasValue)
                query = query.Where(s => s.CityId == queryObject.CityId);

            if (queryObject.ProvinceId.HasValue)
                query = query.Where(s => s.City.ProvinceId == queryObject.ProvinceId);

            if (queryObject.IdentityTypeId.HasValue)
                query = query.Where(s => s.IdentityTypeId == queryObject.IdentityTypeId);

            if (queryObject.PartyType.HasValue)
                query = query.Where(p => p.PartyType == queryObject.PartyType);

            if (!string.IsNullOrEmpty(queryObject.Name))
                query = query.Where(s => s.Name == queryObject.Name);

            if (!string.IsNullOrEmpty(queryObject.IdentityValue))
                query = query.Where(p => p.IdentityValue == queryObject.IdentityValue);


            var columnsMap = new Dictionary<string, Expression<Func<Party, object>>>()
            {
                ["name"] = v => v.Name,
                ["identityType"] = v => v.IdentityType.Name,
                ["province"] = v => v.City.Province.Name,
                ["city"] = v => v.City.Name
            };

            query = query.ApplySorting(queryObject, columnsMap);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObject);

            result.Items = await query.ToListAsync();

            return mapper.Map<QueryResult<Party>, QueryResultDto<PartyListItemDto>>(result);
        }

        public async Task<PartyDetailsDto> GetAsync(int id)
        {
            var party = await db.Parties
               .Include(a => a.IdentityType)
               .Include(s => s.City)
                .ThenInclude(p => p.Province)
               .AsNoTracking()
               .FirstOrDefaultAsync(s => s.Id == id);

            return mapper.Map<PartyDetailsDto>(party);
        }

        public async Task AddAsync(PartyDto partyDto)
        {
            var entityToAdd = mapper.Map<Party>(partyDto);

            entityToAdd.CreatedBy = CurrentUser.UserId;
            entityToAdd.CreatedOn = DateTime.Now;
            entityToAdd.IdentityType = null;

            await db.Parties.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            mapper.Map(entityToAdd, partyDto);
        }

        public async Task EditAsync(PartyDto partyDto)
        {
            var entityToUpdate = await db.Parties.AsNoTracking().FirstOrDefaultAsync(p => p.Id == partyDto.Id);

            mapper.Map(partyDto, entityToUpdate);

            entityToUpdate.UpdatedBy = CurrentUser.UserId;
            entityToUpdate.UpdatedOn = DateTime.Now;

            await db.SaveChangesAsync();

            mapper.Map(entityToUpdate, partyDto);
        }

        public async Task RemoveAsync(int id)
        {
            var entityToDelete = await db.Parties.FindAsync(id);

            entityToDelete.UpdatedBy = CurrentUser.UserId;
            entityToDelete.UpdatedOn = DateTime.Now;
            entityToDelete.IsDeleted = true;

            await db.SaveChangesAsync();
        }

        public async Task<bool> IsPartyExist(PartyDto partyDto)
        {
            var entity = db.Parties;
            if (partyDto.PartyType == PartyTypes.Person)
            {
                return await entity.AnyAsync(p => p.IdentityValue == partyDto.IdentityValue && p.IdentityTypeId == partyDto.IdentityTypeId);
            }
            else if (partyDto.PartyType == PartyTypes.GovernmentalEntity)
            {
                return await entity.AnyAsync(p => p.Name == partyDto.Name);
            }
            else if (partyDto.PartyType == PartyTypes.CompanyOrInstitution)
            {
                return await entity.AnyAsync(p => p.CommertialRegistrationNumber == partyDto.CommertialRegistrationNumber);
            }

            return false;
        }
    }
}
