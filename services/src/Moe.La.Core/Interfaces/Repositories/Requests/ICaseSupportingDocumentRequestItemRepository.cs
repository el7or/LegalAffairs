using Moe.La.Core.Dtos;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface ICaseSupportingDocumentRequestItemRepository
    {
        Task AddAsync(CaseSupportingDocumentRequestItemDto CaseSupportingDocumentRequestItemDto);

        Task EditAsync(CaseSupportingDocumentRequestItemDto CaseSupportingDocumentRequestItemDto);

        Task RemoveAsync(int id);
    }
}
