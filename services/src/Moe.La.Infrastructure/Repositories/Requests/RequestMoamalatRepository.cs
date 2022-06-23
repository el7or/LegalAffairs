using AutoMapper;
using Moe.La.Core.Dtos.Requests;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Infrastructure.DbContexts;
using System;
using System.Threading.Tasks;

namespace Moe.La.Infrastructure.Repositories
{
    public class RequestMoamalatRepository : RepositoryBase, IRequestMoamalatRepository
    {
        public RequestMoamalatRepository(IMapper mapper, LaDbContext context, IUserProvider userProvider)
            : base(context, mapper, userProvider)
        {
        }

        public async Task AddAsync(RequestMoamalatDto requestMoamalatDto)
        {
            var entityToAdd = mapper.Map<RequestsMoamalat>(requestMoamalatDto);

            entityToAdd.CreatedOn = DateTime.Now;
            entityToAdd.CreatedBy = CurrentUser.UserId;

            await db.RequestsMoamalat.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            mapper.Map<RequestMoamalatDto>(entityToAdd);
        }

    }
}
