using Microsoft.Extensions.Logging;
using Moe.La.Core.Dtos;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using Moe.La.ServiceInterface.Validators.Others;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(INotificationRepository notificationRepository, ILogger<NotificationService> logger)
        {
            _notificationRepository = notificationRepository;
            _logger = logger;
        }

        //public async Task<ReturnResult<QueryResultDto<NotificationListItemDto>>> GetAllAsync(NotificationSystemQueryObject queryObject)
        //{
        //    try
        //    {
        //        var entities = await _notificationRepository.GetAllAsync(queryObject);

        //        return new ReturnResult<QueryResultDto<NotificationListItemDto>>
        //        {
        //            IsSuccess = true,
        //            StatusCode = HttpStatuses.Status200OK,
        //            Data = entities
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message, queryObject);

        //        return new ReturnResult<QueryResultDto<NotificationListItemDto>>
        //        {
        //            IsSuccess = false,
        //            StatusCode = HttpStatuses.Status500InternalServerError,
        //            ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
        //        };
        //    }
        //}

        //public async Task<ReturnResult<NotificationDto>> GetAsync(int id)
        //{
        //    try
        //    {
        //        var entitiy = await _notificationRepository.GetAsync(id);

        //        return new ReturnResult<NotificationDto>
        //        {
        //            IsSuccess = true,
        //            StatusCode = HttpStatuses.Status200OK,
        //            Data = entitiy
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message, id);

        //        return new ReturnResult<NotificationDto>
        //        {
        //            IsSuccess = false,
        //            StatusCode = HttpStatuses.Status500InternalServerError,
        //            ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
        //        };
        //    }
        //}

        public async Task<ReturnResult<NotificationDto>> AddAsync(NotificationDto model)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new NotificationValidator(), model);
                if (!validationResult.IsValid)
                {
                    return new ReturnResult<NotificationDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = validationResult.Errors,
                        Data = null
                    };
                }

                await _notificationRepository.AddAsync(model);

                return new ReturnResult<NotificationDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<NotificationDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        //public async Task<ReturnResult<bool>> ReadAsync(int id)
        //{
        //    try
        //    {
        //        //await _notificationRepository.ReadAsync(id);

        //        return new ReturnResult<bool>
        //        {
        //            IsSuccess = true,
        //            StatusCode = HttpStatuses.Status201Created,
        //            Data = true
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message, id);

        //        return new ReturnResult<bool>
        //        {
        //            IsSuccess = false,
        //            StatusCode = HttpStatuses.Status500InternalServerError,
        //            ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
        //        };
        //    }
        //}

        //public async Task<ReturnResult<NotificationDto>> EditAsync(NotificationDto model)
        //{
        //    try
        //    {
        //        var validationResult = ValidationResult.CheckModelValidation(new NotificationValidator(), model);
        //        if (!validationResult.IsValid)
        //        {
        //            return new ReturnResult<NotificationDto>
        //            {
        //                IsSuccess = false,
        //                StatusCode = HttpStatuses.Status400BadRequest,
        //                ErrorList = validationResult.Errors,
        //                Data = null
        //            };
        //        }

        //        await _notificationRepository.EditAsync(model);

        //        return new ReturnResult<NotificationDto>
        //        {
        //            IsSuccess = true,
        //            StatusCode = HttpStatuses.Status201Created,
        //            Data = model
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message, model);

        //        return new ReturnResult<NotificationDto>
        //        {
        //            IsSuccess = false,
        //            StatusCode = HttpStatuses.Status500InternalServerError,
        //            ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
        //        };
        //    }
        //}

        //public async Task<ReturnResult<bool>> RemoveAsync(int id)
        //{
        //    try
        //    {

        //        await _notificationRepository.RemoveAsync(id);

        //        return new ReturnResult<bool>
        //        {
        //            IsSuccess = true,
        //            StatusCode = HttpStatuses.Status201Created,
        //            Data = true
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message, id);

        //        return new ReturnResult<bool>
        //        {
        //            IsSuccess = false,
        //            StatusCode = HttpStatuses.Status500InternalServerError,
        //            ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
        //        };
        //    }
        //}
    }
}
