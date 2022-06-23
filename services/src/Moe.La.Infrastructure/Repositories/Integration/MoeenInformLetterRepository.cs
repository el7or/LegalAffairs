using AutoMapper;
using Moe.La.Core.Entities.Integration.Moeen;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models.Integration.Moeen;
using Moe.La.Infrastructure.DbContexts;
using System.Threading.Tasks;

namespace Moe.La.Infrastructure.Repositories
{
    public class MoeenInformLetterRepository : RepositoryBase, IMoeenInformLetterRepository
    {
        public MoeenInformLetterRepository(IMapper mapper, LaDbContext context, IUserProvider userProvider)
            : base(context, mapper, userProvider)
        {

        }

        public async Task AddAsync(InformLetterInfoStructureModel informLetter)
        {
            var entityToAdd = mapper.Map<InformLetter>(informLetter);
            entityToAdd.CreatedBy = CurrentUser.UserId;
            await db.InformLetters.AddAsync(entityToAdd);
            await db.SaveChangesAsync();
        }
    }
}
