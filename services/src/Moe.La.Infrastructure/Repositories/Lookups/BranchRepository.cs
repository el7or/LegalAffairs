using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
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
    public class BranchRepository : RepositoryBase, IBranchRepository
    {
        private readonly IDistributedCache _cache;
        private const string CacheKey = "Lookup_General_Managements";

        public BranchRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider, IDistributedCache cache)
            : base(context, mapperConfig, userProvider)
        {
            _cache = cache;
        }

        public async Task<IList<BranchListItemDto>> GetAllAsync()
        {
            var departments = await db.Branches
                .Include(a => a.Parent)
                .OrderBy(n => n.Parent)
                .AsNoTracking()
                .ToListAsync();

            return mapper.Map<IList<BranchListItemDto>>(departments);
        }

        public async Task<QueryResultDto<BranchListItemDto>> GetAllAsync(BranchQueryObject queryObject)
        {
            QueryResult<Branch> result = new();
            IQueryable<Branch> query = null;

            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<Branch>>(CacheKey) ?? new List<Branch>();

            if (listValueInCache.Any())
            {
                // Set query data from cache 
                query = listValueInCache.OrderBy(n => n.Name).AsQueryable();
            }
            else
            {
                var entityDbSet = db.Branches.Include(a => a.Parent)
                    .Include(a => a.BranchDepartments);

                // Set query data from database
                query = entityDbSet.OrderBy(n => n.Name).AsQueryable();

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            if (queryObject.IsParent)
            {
                query = query.Where(m => m.ParentId == null);
            }

            if (queryObject.Name != null && queryObject.Name != "")
            {
                query = query.Where(m => m.Name == queryObject.Name);
            }

            var columnsMap = new Dictionary<string, Expression<Func<Branch, object>>>()
            {
                ["parent"] = m => m.ParentId ?? m.Id,
                ["name"] = v => v.Name,
                ["id"] = v => v.Id,
            };

            query = query.ApplySorting(queryObject, columnsMap);
            result.TotalItems = query.Count();
            query = query.ApplyPaging(queryObject);
            result.Items = query.ToList();

            return mapper.Map<QueryResult<Branch>, QueryResultDto<BranchListItemDto>>(result);
        }

        public async Task<BranchDto> GetAsync(int id)
        {
            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<Branch>>(CacheKey) ?? new List<Branch>();

            Branch branch = listValueInCache.FirstOrDefault(m => m.Id == id);

            if (branch is null)
            {
                var entityDbSet = db.Branches.Include(a => a.Parent)
                                                              .Include(a => a.BranchDepartments);

                // Get data from database
                branch = await entityDbSet.FirstOrDefaultAsync(s => s.Id == id);

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            return mapper.Map<BranchDto>(branch);
        }

        public async Task<IList<DepartmentListItemDto>> GetDepartmentsAsync(int id)
        {

            var departments = await db.BranchDepartments
                .Where(m => m.BranchId == id)
                .Select(m => m.Department)
                .ToListAsync();

            if (!departments.Any())
            {
                return null;
            }

            return mapper.Map<ICollection<Department>, IList<DepartmentListItemDto>>(departments);
        }

        public async Task<BranchListItemDto> GetByNameAsync(string name)
        {
            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<Branch>>(CacheKey) ?? new List<Branch>();

            Branch branch = listValueInCache.FirstOrDefault(m => m.Name == name);

            if (branch is null)
            {
                var entityDbSet = db.Branches;

                // Get data from database
                branch = await entityDbSet.FirstOrDefaultAsync(s => s.Name == name);

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.Include(a => a.Parent)
                                                                       .Include(a => a.BranchDepartments).ToListAsync());
            }

            return mapper.Map<BranchListItemDto>(branch);
        }

        public async Task AddAsync(BranchDto branchDto)
        {
            var entityDbSet = db.Branches;

            var entityToAdd = mapper.Map<Branch>(branchDto);
            entityToAdd.Id = (await entityDbSet.IgnoreQueryFilters().MaxAsync(c => (int?)c.Id) ?? 0) + 1;
            entityToAdd.CreatedOn = DateTime.Now;

            if (branchDto.Departments.Any())
            {
                foreach (var department in branchDto.Departments)
                {
                    entityToAdd.BranchDepartments.Add(new BranchesDepartments
                    {
                        BranchId = entityToAdd.Id,
                        DepartmentId = department,
                        CreatedBy = CurrentUser.UserId,
                        CreatedOn = DateTime.Now
                    });
                }
            }
            await entityDbSet.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.Include(a => a.Parent)
                                                                   .Include(a => a.BranchDepartments).ToListAsync());


            mapper.Map(entityToAdd, branchDto);
        }

        public async Task EditAsync(BranchDto branchDto)
        {
            var entityDbSet = db.Branches;
            var entityToUpdate = await entityDbSet.FindAsync(branchDto.Id);
            mapper.Map(branchDto, entityToUpdate);

            // Get Department Branch not in the edited data from the database.
            if (branchDto.Departments.Any())
            {
                List<BranchesDepartments> departmentBranches = new List<BranchesDepartments>();
                foreach (var department in branchDto.Departments)
                {
                    departmentBranches.Add(new BranchesDepartments
                    {
                        BranchId = entityToUpdate.Id,
                        DepartmentId = department,
                        CreatedBy = CurrentUser.UserId,
                        CreatedOn = DateTime.Now
                    });
                }

                // Remove Department Branches not in General Managment edit data.
                var departmentInDbNotInBranch = await db.BranchDepartments.Where(m => m.BranchId == entityToUpdate.Id).ToListAsync();
                db.BranchDepartments.RemoveRange(departmentInDbNotInBranch);

                // Add new Department Branches from General Managment edit data.
                db.BranchDepartments.AddRange(departmentBranches);
            }
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.Include(a => a.Parent)
                                                                    .Include(a => a.BranchDepartments).ToListAsync());


            mapper.Map(entityToUpdate, branchDto);
        }

        public async Task RemoveAsync(int id)
        {
            var entityDbSet = db.Branches;

            var entityToDelete = await entityDbSet.FindAsync(id);
            entityDbSet.Remove(entityToDelete);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.Include(a => a.Parent)
                                                                   .Include(a => a.BranchDepartments).ToListAsync());

        }

        public async Task<bool> IsNameExistsAsync(BranchDto branchDto)
        {
            return await db.Branches
                            .AnyAsync(c => c.Id != branchDto.Id && c.Name == branchDto.Name);
        }
    }
}
