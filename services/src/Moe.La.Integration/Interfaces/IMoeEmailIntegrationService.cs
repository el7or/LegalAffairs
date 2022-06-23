using Moe.La.Core.Models;
using Moe.La.Integration.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.Integration.Interfaces
{
    /// <summary>
    /// MOE email integration service.
    /// </summary>
    public interface IMoeEmailIntegrationService
    {
        /// <summary>
        /// Send and email.
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        Task<ReturnResult<EmailResponseWrapper>> SendAsync(string to, string subject, string body);

        /// <summary>
        /// Send an email.
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        Task<ReturnResult<EmailResponseWrapper>> SendAsync(List<string> to, string subject, string body);

        /// <summary>
        /// Send an email.
        /// </summary>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        Task<ReturnResult<EmailResponseWrapper>> SendAsync(List<string> to, List<string> cc, string subject, string body);

        /// <summary>
        /// Send an email.
        /// </summary>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        /// <param name="bcc"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        Task<ReturnResult<EmailResponseWrapper>> SendAsync(List<string> to, List<string> cc, List<string> bcc, string subject, string body);
    }
}
