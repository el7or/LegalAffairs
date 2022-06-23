//using AutoMapper;
//using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore;
//using Moe.La.Core.Dtos;
//using Moe.La.Core.Entities;
//using Moe.La.Core.Interfaces.Repositories;
//using Moe.La.Infrastructure.DbContexts;
//using Moe.La.Infrastructure.Extensions;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Threading.Tasks;

//namespace Moe.La.Infrastructure.Repositories
//{

//    public class PartyTypeRepository : RepositoryBase, IPartyTypeRepository
//    {

//        public PartyTypeRepository(CommandDbContext context, IMapper mapperConfig, IUserProvider userProvider) : base(context, mapperConfig, userProvider)
//        {
//        }

//        public async Task<QueryResultDto<PartyTypeListItemDto>> GetAllAsync(QueryObject queryObject)
//        {
//            var result = new QueryResult<PartyType>();

//            var query = commandDb.PartyTypes
//                .OrderBy(n => n.Name)
//                .AsQueryable();

//            var columnsMap = new Dictionary<string, Expression<Func<PartyType, object>>>()
//            {
//                ["name"] = v => v.Name
//            };

//            query = query.ApplySorting(queryObject, columnsMap);

//            result.TotalItems = await query.CountAsync();

//            query = query.ApplyPaging(queryObject);

//            result.Items = await query.ToListAsync();

//            return mapper.Map<QueryResult<PartyType>, QueryResultDto<PartyTypeListItemDto>>(result);
//        }

//        public async Task<PartyTypeListItemDto> GetAsync(int id)
//        {
//            var PartyType = await commandDb.PartyTypes
//                .AsNoTracking()
//                .FirstOrDefaultAsync(m => m.Id == id);

//            return mapper.Map<PartyTypeListItemDto>(PartyType);
//        }

//        public async Task AddAsync(PartyTypeDto partyTypeDto)
//        {
//            var entityToAdd = mapper.Map<PartyType>(partyTypeDto);
//            entityToAdd.Id = (await commandDb.PartyTypes.IgnoreQueryFilters().MaxAsync(c => (int?)c.Id) ?? 0) + 1;
//            entityToAdd.CreatedOn = DateTime.Now;
//            await commandDb.PartyTypes.AddAsync(entityToAdd);
//            await commandDb.SaveChangesAsync();
//            mapper.Map(entityToAdd, partyTypeDto);
//        }

//        public async Task EditAsync(PartyTypeDto partyTypeDto)
//        {
//            var entityToUpdate = await commandDb.PartyTypes.FindAsync(partyTypeDto.Id);
//            mapper.Map(partyTypeDto, entityToUpdate);
//            await commandDb.SaveChangesAsync();
//            mapper.Map(entityToUpdate, partyTypeDto);
//        }

//        public async Task RemoveAsync(int id)
//        {
//            var entityToDelete = await commandDb.PartyTypes.FindAsync(id);
//            entityToDelete.IsDeleted = true;
//            await commandDb.SaveChangesAsync();
//        }
//    }
//}
