using Moe.La.Core.Dtos.Requests;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IRequestMoamalatRepository
    {
        Task AddAsync(RequestMoamalatDto requestMoamalatDto);
    }
}
