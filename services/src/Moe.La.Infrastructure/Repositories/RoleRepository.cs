using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Infrastructure.DbContexts;
using Moe.La.Infrastructure.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Moe.La.Infrastructure.Repositories
{
    public class RoleRepository : RepositoryBase, IRoleRepository
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleRepository(RoleManager<AppRole> roleManager, LaDbContext context, IMapper mapperConfig, IUserProvider userProvider)
            : base(context, mapperConfig, userProvider)
        {
            _roleManager = roleManager;
        }

        public async Task<QueryResultDto<RoleListItemDto>> GetAllAsync(QueryObject queryObject)
        {
            var result = new QueryResult<AppRole>();

            var query = _roleManager.Roles
                .AsNoTracking()
                .AsQueryable();


            query = query.OrderBy(r => r.Priority);


            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObject);

            result.Items = await query.ToListAsync();

            return mapper.Map<QueryResult<AppRole>, QueryResultDto<RoleListItemDto>>(result);
        }

        public async Task<RoleListItemDto> GetAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());

            return mapper.Map<RoleListItemDto>(role);
        }

        public async Task AddAsync(RoleDto roleDto)
        {
            var entityToAdd = mapper.Map<AppRole>(roleDto);

            entityToAdd.Id = Guid.NewGuid();

            var result = await _roleManager.CreateAsync(entityToAdd);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException();
            }

            mapper.Map(entityToAdd, roleDto);
        }

        public async Task EditAsync(RoleDto roleDto)
        {
            var entityToUpdate = await _roleManager.FindByIdAsync(roleDto.Id.ToString());

            mapper.Map(roleDto, entityToUpdate);

            entityToUpdate.Name = roleDto.Name;
            entityToUpdate.NormalizedName = roleDto.Name.ToUpper();

            await _roleManager.UpdateAsync(entityToUpdate);

            mapper.Map(entityToUpdate, roleDto);
        }

        public async Task RemoveAsync(Guid id)
        {
            var entityToDelete = await _roleManager.FindByIdAsync(id.ToString());

            await _roleManager.DeleteAsync(entityToDelete);
        }
    }
}
