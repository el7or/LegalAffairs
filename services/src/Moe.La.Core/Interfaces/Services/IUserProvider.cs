using Moe.La.Core.Models;

namespace Moe.La.Core.Interfaces.Services
{
    /// <summary>
    /// Provides the current logged in user information.
    /// </summary>
    public interface IUserProvider
    {
        /// <summary>
        /// The current user object information.
        /// </summary>
        CurrentUser CurrentUser { get; }
    }
}
