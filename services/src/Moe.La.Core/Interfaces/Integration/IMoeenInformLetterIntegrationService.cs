using Moe.La.Core.Models;
using Moe.La.Core.Models.Integration.Moeen;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    /// <summary>
    /// Moeen Inform Letter integration service.
    /// </summary>
    /// <remarks>Provided by Yesser</remarks>
    public interface IMoeenInformLetterIntegrationService
    {
        /// <summary>
        /// Add new inform letter.
        /// </summary>
        /// <param name="informLetter">Inform letter object to be saved.</param>
        /// <returns></returns>
        Task<ReturnResult<bool>> AddAsync(InformLetterInfoStructureModel informLetter);
    }
}
