using Microsoft.Extensions.Logging;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
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
    public class HearingUpdateService : IHearingUpdateService
    {
        private readonly IHearingUpdateRepository _hearingUpdateRepository;
        private readonly IAttachmentService _attachmentService;
        private readonly ILogger<HearingUpdateService> _logger;

        public HearingUpdateService(
            IHearingUpdateRepository hearingUpdateRepository,
            IAttachmentService attachmentService,
            ILogger<HearingUpdateService> logger)
        {
            _hearingUpdateRepository = hearingUpdateRepository;
            _attachmentService = attachmentService;
            _logger = logger;
        }


        #region HearingUpdate
        public async Task<ReturnResult<QueryResultDto<HearingUpdateListItemDto>>> GetAllAsync(HearingUpdateQueryObject queryObject)
        {
            try
            {
                var entities = await _hearingUpdateRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<HearingUpdateListItemDto>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entities
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, queryObject);

                return new ReturnResult<QueryResultDto<HearingUpdateListItemDto>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string>() { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<HearingUpdateDetailsDto>> GetAsync(int id)
        {
            try
            {
                var entitiy = await _hearingUpdateRepository.GetAsync(id);

                return new ReturnResult<HearingUpdateDetailsDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entitiy
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<HearingUpdateDetailsDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string>() { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<HearingUpdateDto>> AddAsync(HearingUpdateDto model)
        {
            try
            {
                var errors = new List<string>();

                var validationResult = ValidationResult.CheckModelValidation(new HearingUpdateValidator(), model);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                await _hearingUpdateRepository.AddAsync(model);

                await _attachmentService.UpdateListAsync(model.Attachments);

                return new ReturnResult<HearingUpdateDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<HearingUpdateDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string>() { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<HearingUpdateDto>> EditAsync(HearingUpdateDto model)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new HearingUpdateValidator(), model);
                if (!validationResult.IsValid)
                {
                    return new ReturnResult<HearingUpdateDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = validationResult.Errors,
                        Data = null
                    };
                }

                await _hearingUpdateRepository.EditAsync(model);

                await _attachmentService.UpdateListAsync(model.Attachments);

                return new ReturnResult<HearingUpdateDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<HearingUpdateDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string>() { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        #endregion
    }
}
