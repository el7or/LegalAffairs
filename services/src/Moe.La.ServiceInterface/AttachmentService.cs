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
    public class AttachmentService : IAttachmentService
    {
        private readonly IAttachmentRepository _attachmentRepository;
        private readonly ILogger<AttachmentService> _logger;

        public AttachmentService(IAttachmentRepository attachmentRepository, ILogger<AttachmentService> logger)
        {
            _attachmentRepository = attachmentRepository;
            _logger = logger;
        }

        public async Task<ReturnResult<QueryResultDto<AttachmentListItemDto>>> GetAllAsync(AttachmentQueryObject queryObject)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new AttachmentQueryObjectValidator(), queryObject);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<QueryResultDto<AttachmentListItemDto>>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                var attachments = await _attachmentRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<AttachmentListItemDto>>(true, HttpStatuses.Status200OK, attachments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, queryObject);

                return new ReturnResult<QueryResultDto<AttachmentListItemDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<AttachmentListItemDto>> AddAsync(AttachmentDto attachment)
        {
            try
            {
                var data = await _attachmentRepository.AddAsync(attachment);

                return new ReturnResult<AttachmentListItemDto>(true, HttpStatuses.Status201Created, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new ReturnResult<AttachmentListItemDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> UpdateListAsync(List<AttachmentDto> attachments)
        {
            try
            {
                await _attachmentRepository.UpdateListAsync(attachments);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> RemoveAsync(Guid id)
        {
            try
            {
                await _attachmentRepository.RemoveAsync(id);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<Task> Cleanup()
        {
            try
            {
                _logger.LogInformation($"{nameof(Cleanup)} job is starting...");

                await _attachmentRepository.Cleanup();

                _logger.LogInformation($"{nameof(Cleanup)} job ended successfully.");

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Task.FromException(ex);
            }
        }
    }
}
