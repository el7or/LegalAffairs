using AutoMapper;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Infrastructure.DbContexts;
using System.Threading.Tasks;

namespace Moe.La.Infrastructure.Repositories
{
    public class CaseSupportingDocumentRequestItemRepository : RepositoryBase, ICaseSupportingDocumentRequestItemRepository
    {
        public CaseSupportingDocumentRequestItemRepository(LaDbContext commandDb, IMapper mapperConfig, IUserProvider userProvider)
           : base(commandDb, mapperConfig, userProvider)
        {
        }
        public async Task AddAsync(CaseSupportingDocumentRequestItemDto documentRequestItemDto)
        {
            var entityToAdd = mapper.Map<CaseSupportingDocumentRequestItem>(documentRequestItemDto);

            await db.DocumentRequestItems.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            mapper.Map(entityToAdd, documentRequestItemDto);
        }

        public async Task EditAsync(CaseSupportingDocumentRequestItemDto documentRequestItemDto)
        {
            var entityToUpdate = await db.DocumentRequestItems.FindAsync(documentRequestItemDto.Id);
            mapper.Map(documentRequestItemDto, entityToUpdate);
            await db.SaveChangesAsync();
            mapper.Map(entityToUpdate, documentRequestItemDto);
        }

        public async Task RemoveAsync(int id)
        {
            var entityToDelete = await db.DocumentRequestItems.FindAsync(id);
            await db.SaveChangesAsync();
        }
    }
}
