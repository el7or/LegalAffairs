using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface ILetterTemplateRepository
    {
        Task<QueryResultDto<LetterTemplateDto>> GetAllAsync(TemplateQueryObject queryObject);

        Task<LetterTemplateDto> GetAsync(int id);

        Task AddAsync(LetterTemplateDto template);

        Task EditAsync(LetterTemplateDto template);

        Task RemoveAsync(int id);
    }
}
