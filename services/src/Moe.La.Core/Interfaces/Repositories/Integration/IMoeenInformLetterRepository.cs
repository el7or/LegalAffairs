using Moe.La.Core.Models.Integration.Moeen;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    /// <summary>
    /// Moeen Inform Letter Repository.
    /// </summary>
    public interface IMoeenInformLetterRepository
    {
        Task AddAsync(InformLetterInfoStructureModel informLetter);
    }
}