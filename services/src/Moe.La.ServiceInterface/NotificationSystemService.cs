using Microsoft.Extensions.Logging;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class NotificationSystemService : INotificationSystemService
    {
        private readonly INotificationSystemRepository _notificationSystemRepository;
        private readonly ILogger<NotificationSystemService> _logger;

        public NotificationSystemService(INotificationSystemRepository notificationSystemRepository, ILogger<NotificationSystemService> logger)
        {
            _notificationSystemRepository = notificationSystemRepository;
            _logger = logger;
        }

        public async Task<ReturnResult<QueryResultDto<NotificationSystemListItemDto>>> GetAllAsync(NotificationSystemQueryObject queryObject)
        {
            try
            {
                var entities = await _notificationSystemRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<NotificationSystemListItemDto>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entities
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, queryObject);

                return new ReturnResult<QueryResultDto<NotificationSystemListItemDto>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<NotificationSystemDetailsDto>> GetAsync(int id)
        {
            try
            {
                var entitiy = await _notificationSystemRepository.GetAsync(id);

                return new ReturnResult<NotificationSystemDetailsDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entitiy
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<NotificationSystemDetailsDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }


        public async Task<ReturnResult<bool>> ReadAsync(int id)
        {
            try
            {
                await _notificationSystemRepository.ReadAsync(id);

                return new ReturnResult<bool>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<bool>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }


    }
}
