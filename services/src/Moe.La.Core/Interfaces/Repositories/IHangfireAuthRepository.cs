using Moe.La.Core.Entities;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IHangfireAuthRepository
    {

        Task<AppUser> GetAdminByUserName(string userName);
    }
}