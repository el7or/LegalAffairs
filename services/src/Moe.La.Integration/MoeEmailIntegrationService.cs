using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moe.La.Common;
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
    public class MoeEmailIntegrationService : IMoeEmailIntegrationService
    {
        private readonly ILogger<MoeEmailIntegrationService> _logger;

        public MoeEmailIntegrationService(IOptions<MoeEmailOptions> options, ILogger<MoeEmailIntegrationService> logger)
        {
            _logger = logger;
            Endpoint = options.Value.Endpoint;
            Username = options.Value.Username;
            Password = options.Value.Password;
            From = options.Value.From;
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
        /// Email from account.
        /// </summary>
        public string From { get; }


        public async Task<ReturnResult<EmailResponseWrapper>> SendAsync(string to, string subject, string body)
        {
            List<string> errors = new();

            if (string.IsNullOrWhiteSpace(to))
            {
                throw new ArgumentNullException(nameof(to), $"Parameter '{nameof(to)}' doesn't have a valid value.");
            }

            if (string.IsNullOrWhiteSpace(subject))
            {
                errors.Add("موضوع البريد الإلكتروني غير صالح");
            }

            if (string.IsNullOrWhiteSpace(body))
            {
                errors.Add("نص البريد الإلكتروني غير صالح");
            }

            if (!ValidatorsHelper.IsValidEmail(to))
            {
                _logger.LogError($"Failed to send an email to an invalid email address: {to}");
                errors.Add($"البريد الإلكتروني '{to}' غير صالح");
            }

            if (errors.Any())
            {
                return new ReturnResult<EmailResponseWrapper>(false, HttpStatuses.Status400BadRequest, errors);
            }

            var emailRequest = new EmailRequest
            {
                FromAddress = From,
                ToAddresses = new ToAddresses { Address = new List<string> { to } },
                Subject = subject,
                Body = body,
                CCAddresses = new CCAddresses { Address = new List<string>() },
                BCCAddresses = new BCCAddresses { Address = new List<string>() }
            };

            return await Process(emailRequest);
        }

        public async Task<ReturnResult<EmailResponseWrapper>> SendAsync(List<string> to, string subject, string body)
        {
            List<string> errors = new();

            if (to is null || !to.Any())
            {
                throw new ArgumentException($"Parameter '{nameof(to)}' doesn't have a valid value.");
            }

            if (string.IsNullOrWhiteSpace(subject))
            {
                errors.Add("موضوع البريد الإلكتروني غير صالح");
            }

            if (string.IsNullOrWhiteSpace(body))
            {
                errors.Add("نص البريد الإلكتروني غير صالح");
            }

            foreach (var email in to)
            {
                if (!ValidatorsHelper.IsValidEmail(email))
                {
                    _logger.LogError($"Failed to send an email to an invalid email address: {email}");
                    errors.Add($"البريد الإلكتروني '{email}' غير صالح");
                }
            }

            if (errors.Any())
            {
                return new ReturnResult<EmailResponseWrapper>(false, HttpStatuses.Status400BadRequest, errors);
            }

            var emailRequest = new EmailRequest
            {
                FromAddress = From,
                ToAddresses = new ToAddresses { Address = to },
                Subject = subject,
                Body = body,
                CCAddresses = new CCAddresses { Address = new List<string>() },
                BCCAddresses = new BCCAddresses { Address = new List<string>() }
            };

            return await Process(emailRequest);
        }

        public async Task<ReturnResult<EmailResponseWrapper>> SendAsync(List<string> to, List<string> cc, string subject, string body)
        {
            List<string> errors = new();

            if (to is null || !to.Any())
            {
                throw new ArgumentException($"Parameter '{nameof(to)}' doesn't have a valid value.");
            }

            if (cc is null || !cc.Any())
            {
                throw new ArgumentException($"Paramter '{nameof(cc)}' doesn't have a valid value.");
            }

            if (string.IsNullOrWhiteSpace(subject))
            {
                errors.Add("موضوع البريد الإلكتروني غير صالح");
            }

            if (string.IsNullOrWhiteSpace(body))
            {
                errors.Add("نص البريد الإلكتروني غير صالح");
            }

            List<string> emailsToValidate = new();
            emailsToValidate.AddRange(to);
            emailsToValidate.AddRange(cc);

            foreach (var email in emailsToValidate)
            {
                if (!ValidatorsHelper.IsValidEmail(email))
                {
                    _logger.LogError($"Failed to send an email to an invalid email address: {email}");
                    errors.Add($"البريد الإلكتروني '{email}' غير صالح");
                }
            }

            if (errors.Any())
            {
                return new ReturnResult<EmailResponseWrapper>(false, HttpStatuses.Status400BadRequest, errors);
            }

            var emailRequest = new EmailRequest
            {
                FromAddress = From,
                ToAddresses = new ToAddresses { Address = to },
                CCAddresses = new CCAddresses { Address = cc },
                BCCAddresses = new BCCAddresses { Address = new List<string>() },
                Subject = subject,
                Body = body
            };

            return await Process(emailRequest);
        }

        public async Task<ReturnResult<EmailResponseWrapper>> SendAsync(List<string> to, List<string> cc, List<string> bcc, string subject, string body)
        {
            List<string> errors = new();

            if (to is null || !to.Any())
            {
                throw new ArgumentException($"Parameter '{nameof(to)}' doesn't have a valid value.");
            }

            if (cc is null || !cc.Any())
            {
                throw new ArgumentException($"Paramter '{nameof(cc)}' doesn't have a valid value.");
            }

            if (bcc is null || !bcc.Any())
            {
                throw new ArgumentException($"Paramter '{nameof(bcc)}' doesn't have a valid value.");
            }

            if (string.IsNullOrWhiteSpace(subject))
            {
                errors.Add("موضوع البريد الإلكتروني غير صالح");
            }

            if (string.IsNullOrWhiteSpace(body))
            {
                errors.Add("نص البريد الإلكتروني غير صالح");
            }

            List<string> emailsToValidate = new();
            emailsToValidate.AddRange(to);
            emailsToValidate.AddRange(cc);
            emailsToValidate.AddRange(bcc);

            foreach (var email in emailsToValidate)
            {
                if (!ValidatorsHelper.IsValidEmail(email))
                {
                    _logger.LogError($"Failed to send an email to an invalid email address: {email}");
                    errors.Add($"البريد الإلكتروني '{email}' غير صالح");
                }
            }

            if (errors.Any())
            {
                return new ReturnResult<EmailResponseWrapper>(false, HttpStatuses.Status400BadRequest, errors);
            }

            var emailRequest = new EmailRequest
            {
                FromAddress = From,
                ToAddresses = new ToAddresses { Address = to },
                CCAddresses = new CCAddresses { Address = cc },
                BCCAddresses = new BCCAddresses { Address = bcc },
                Subject = subject,
                Body = body
            };

            return await Process(emailRequest);
        }

        private async Task<ReturnResult<EmailResponseWrapper>> Process(EmailRequest emailRequest)
        {
            try
            {
                using (var client = GetClient())
                using (var content = GetStringContent(emailRequest))
                {
                    var response = await client.PostAsync(Endpoint, content);
                    var result = await response.Content.ReadFromJsonAsync<EmailResponseWrapper>();

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError(await response.Content.ReadAsStringAsync());
                        return new ReturnResult<EmailResponseWrapper>(false, HttpStatuses.Status400BadRequest, result);
                    }

                    return new ReturnResult<EmailResponseWrapper>(true, HttpStatuses.Status200OK, result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, emailRequest);
                return new ReturnResult<EmailResponseWrapper>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Username", Username);
            client.DefaultRequestHeaders.Add("Password", Password);

            return client;
        }

        private StringContent GetStringContent(EmailRequest emailRequest)
        {
            var emailString = JsonSerializer.Serialize(emailRequest);

            return new StringContent(emailString, Encoding.UTF8, "application/json");
        }

    }
}
