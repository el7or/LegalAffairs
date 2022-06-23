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
    public class NotificationSMSService : INotificationSMSService
    {
        private readonly INotificationSMSRepository _notificationSMSRepository;
        private readonly IMoeSmsIntegrationService _moeSmsIntegrationService;
        private readonly ILogger<NotificationSMSService> _logger;

        public NotificationSMSService(INotificationSMSRepository NotificationSMSRepository, IMoeSmsIntegrationService moeSmsIntegrationService, ILogger<NotificationSMSService> logger)
        {
            _notificationSMSRepository = NotificationSMSRepository;
            _moeSmsIntegrationService = moeSmsIntegrationService;
            _logger = logger;
        }

        public async Task<ReturnResult<QueryResultDto<NotificationSMSListItemDto>>> GetAllAsync(NotificationSMSQueryObject queryObject)
        {
            try
            {
                var entities = await _notificationSMSRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<NotificationSMSListItemDto>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entities
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, queryObject);

                return new ReturnResult<QueryResultDto<NotificationSMSListItemDto>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task SendSMSs()
        {
            try
            {
                _logger.LogInformation($"{nameof(SendSMSs)} job is running...");

                var notSentSMSs = await _notificationSMSRepository.GetAllAsync(new NotificationSMSQueryObject { IsSent = false });

                foreach (var notificationSMS in notSentSMSs.Items)
                {

                    var res = await _moeSmsIntegrationService.SendAsync(notificationSMS.PhoneNumber, notificationSMS.Text, true);

                    if (res.IsSuccess)
                    {
                        await _notificationSMSRepository.SentSuccessufullyAsync(notificationSMS);
                    }
                }

                _logger.LogInformation($"{nameof(SendSMSs)} job ended successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
