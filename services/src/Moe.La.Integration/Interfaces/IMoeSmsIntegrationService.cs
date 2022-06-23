using Moe.La.Core.Models;
using Moe.La.Integration.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.Integration.Interfaces
{
    public interface IMoeSmsIntegrationService
    {
        /// <summary>
        /// Send SMS.
        /// </summary>
        /// <param name="mobileNumbers"></param>
        /// <param name="message"></param>
        /// <param name="isArabic"></param>
        /// <returns></returns>
        Task<ReturnResult<SmsResponse>> SendAsync(string mobileNumber, string message, bool isArabic);

        /// <summary>
        /// Send SMS.
        /// </summary>
        /// <param name="mobileNumbers">Mobile numbers to send SMS.</param>
        /// <param name="message">The message to send.</param>
        /// <param name="isArabic">Determine whether or not the message text in Arabic.</param>
        /// <returns></returns>
        Task<ReturnResult<SmsResponse>> SendAsync(List<string> mobileNumbers, string message, bool isArabic);
    }
}
