using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface ILetterTemplateService
    {
        Task<ReturnResult<QueryResultDto<LetterTemplateDto>>> GetAllAsync(TemplateQueryObject queryObject);

        Task<ReturnResult<LetterTemplateDto>> GetAsync(int id);

        Task<ReturnResult<LetterTemplateDto>> AddAsync(LetterTemplateDto model);

        Task<ReturnResult<LetterTemplateDto>> EditAsync(LetterTemplateDto model);

        Task<ReturnResult<bool>> RemoveAsync(int id);
    }
}
