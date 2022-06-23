using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IInvestiationRecordPartyRepository
    {
        Task<bool> CheckPartyExist(string identityNumber, int? investigationRecordId);
    }
}
