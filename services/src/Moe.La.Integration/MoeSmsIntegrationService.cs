using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moe.La.Core.Enums;
using Moe.La.Core.Models;
using Moe.La.Integration.Interfaces;
using Moe.La.Integration.Models;
using Moe.La.Integration.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Moe.La.Integration
{
    public class MoeSmsIntegrationService : IMoeSmsIntegrationService
    {
        private readonly ILogger<MoeSmsIntegrationService> _logger;

        public MoeSmsIntegrationService(IOptions<MoeSmsOptions> options, ILogger<MoeSmsIntegrationService> logger)
        {
            _logger = logger;
            Endpoint = options.Value.Endpoint;
            Username = options.Value.Username;
            Password = options.Value.Password;
            Sender = options.Value.Sender;
            CheckActivation = options.Value.CheckActivation;
        }

        /// <summary>
        /// Send email endpoint.
        /// </summary>
        public string Endpoint { get; }

        /// <summary>
        /// Service account username.
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// Service account password.
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// The name of the sender.
        /// </summary>
        /// <remarks>Eg. MOE</remarks>
        public string Sender { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool CheckActivation { get; }

        public async Task<ReturnResult<SmsResponse>> SendAsync(string mobileNumber, string message, bool isArabic = true)
        {
            if (string.IsNullOrWhiteSpace(mobileNumber))
            {
                throw new ArgumentException($"Parameter '{nameof(mobileNumber)}' doesn't have a valid value.");
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException($"Parameter '{nameof(message)}' doesn't have a valid value.");
            }

            mobileNumber = PrepareMobileNumber(mobileNumber);

            var smsRequest = new SmsRequest()
            {
                Sender = Sender,
                CheckActivation = CheckActivation,
                IsArabic = isArabic,
                Text = message,
                ToList = new SMSToList { ToNumber = new List<string> { mobileNumber } }
            };

            return await Process(smsRequest);
        }

        public async Task<ReturnResult<SmsResponse>> SendAsync(List<string> mobileNumbers, string message, bool isArabic = true)
        {
            if (mobileNumbers == null || !mobileNumbers.Any())
            {
                throw new ArgumentException($"Parameter '{nameof(mobileNumbers)}' doesn't have a valid value.");
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException($"Parameter '{nameof(message)}' doesn't have a valid value.");
            }

            for (int i = 0; i < mobileNumbers.Count; i++)
            {
                mobileNumbers[i] = PrepareMobileNumber(mobileNumbers[i]);
            }

            var smsRequest = new SmsRequest()
            {
                Sender = Sender,
                CheckActivation = CheckActivation,
                IsArabic = isArabic,
                Text = message,
                ToList = new SMSToList { ToNumber = mobileNumbers }
            };

            return await Process(smsRequest);
        }

        private async Task<ReturnResult<SmsResponse>> Process(SmsRequest smsRequest)
        {
            try
            {
                using (var client = GetClient())
                using (var content = GetStringContent(smsRequest))
                {
                    var response = await client.PostAsync(Endpoint, content);
                    var result = await response.Content.ReadFromJsonAsync<SmsResponse>();

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError(await response.Content.ReadAsStringAsync());
                        return new ReturnResult<SmsResponse>(false, HttpStatuses.Status400BadRequest, result);
                    }

                    return new ReturnResult<SmsResponse>(true, HttpStatuses.Status200OK, result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, smsRequest);
                return new ReturnResult<SmsResponse>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Username", Username);
            client.DefaultRequestHeaders.Add("Password", Password);

            return client;
        }

        private StringContent GetStringContent(SmsRequest smsRequest)
        {
            var smsString = JsonSerializer.Serialize(smsRequest);

            return new StringContent(smsString, Encoding.UTF8, "application/json");
        }

        private string PrepareMobileNumber(string number)
        {
            if (!number.StartsWith('0'))
            {
                number = number.Insert(0, "0");
            }

            return number;
        }
    }
}
