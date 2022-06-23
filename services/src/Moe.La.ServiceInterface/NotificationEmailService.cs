using Microsoft.Extensions.Logging;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using Moe.La.Integration.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class NotificationEmailService : INotificationEmailService
    {
        private readonly INotificationEmailRepository _notificationEmailRepository;
        private readonly IMoeEmailIntegrationService _moeEmailIntegrationService;
        private readonly ILogger<NotificationEmailService> _logger;

        public NotificationEmailService(INotificationEmailRepository notificationEmailRepository, IMoeEmailIntegrationService moeEmailIntegrationService, ILogger<NotificationEmailService> logger)
        {
            _notificationEmailRepository = notificationEmailRepository;
            _moeEmailIntegrationService = moeEmailIntegrationService;
            _logger = logger;
        }

        public async Task<ReturnResult<QueryResultDto<NotificationEmailListItemDto>>> GetAllAsync(NotificationEmailQueryObject queryObject)
        {
            try
            {
                var entities = await _notificationEmailRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<NotificationEmailListItemDto>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entities
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, queryObject);

                return new ReturnResult<QueryResultDto<NotificationEmailListItemDto>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task SendEmails()
        {
            try
            {
                _logger.LogInformation($"{nameof(SendEmails)} job is running...");

                var notSentEmails = await _notificationEmailRepository.GetAllAsync(new NotificationEmailQueryObject { IsSent = false });

                foreach (var email in notSentEmails.Items)
                {

                    var res = await _moeEmailIntegrationService.SendAsync(email.Email, email.Text, email.Text);

                    if (res.IsSuccess)
                    {
                        await _notificationEmailRepository.SentSuccessufullyAsync(email);
                    }
                }

                _logger.LogInformation($"{nameof(SendEmails)} job ended successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
