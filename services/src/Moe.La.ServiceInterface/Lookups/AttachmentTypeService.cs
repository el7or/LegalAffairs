using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using Moe.La.ServiceInterface.Validators.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class AttachmentTypeService : IAttachmentTypeService
    {
        private readonly IAttachmentTypeRepository _attachmentTypeRepository;
        private readonly ILogger<AttachmentTypeService> _logger;

        public AttachmentTypeService(IAttachmentTypeRepository attachmentTypeRepository, ILogger<AttachmentTypeService> logger)
        {
            _attachmentTypeRepository = attachmentTypeRepository;
            _logger = logger;
        }

        public async Task<ReturnResult<QueryResultDto<AttachmentTypeListItemDto>>> GetAllAsync(AttachmentQueryObject queryObject)
        {
            try
            {
                var entities = await _attachmentTypeRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<AttachmentTypeListItemDto>>(true, HttpStatuses.Status200OK, entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, queryObject);

                return new ReturnResult<QueryResultDto<AttachmentTypeListItemDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<AttachmentTypeDetailsDto>> GetAsync(int id)
        {
            try
            {
                var entity = await _attachmentTypeRepository.GetAsync(id);

                return new ReturnResult<AttachmentTypeDetailsDto>(true, HttpStatuses.Status200OK, entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<AttachmentTypeDetailsDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<AttachmentTypeDto>> AddAsync(AttachmentTypeDto model)
        {
            try
            {
                var errors = new List<string>();

                var validationResult = ValidationResult.CheckModelValidation(new AttachmentTypeValidator(), model);

                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                bool isAttachmentTypeExists = await _attachmentTypeRepository.IsNameExistsAsync(model);
                if (isAttachmentTypeExists)
                {
                    errors.Add("نوع المرفق موجود لنفس المجموعة");
                }

                if (errors.Any())
                    return new ReturnResult<AttachmentTypeDto>(false, HttpStatuses.Status400BadRequest, errors);

                await _attachmentTypeRepository.AddAsync(model);

                return new ReturnResult<AttachmentTypeDto>(true, HttpStatuses.Status201Created, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<AttachmentTypeDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<AttachmentTypeDto>> EditAsync(AttachmentTypeDto model)
        {
            try
            {
                var errors = new List<string>();

                var validationResult = ValidationResult.CheckModelValidation(new AttachmentTypeValidator(), model);

                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                bool isAttachmentTypeExists = await _attachmentTypeRepository.IsNameExistsAsync(model);
                if (isAttachmentTypeExists)
                {
                    errors.Add("نوع المرفق موجود لنفس المجموعة");
                }

                if (errors.Any())
                    return new ReturnResult<AttachmentTypeDto>(false, HttpStatuses.Status400BadRequest, errors);

                await _attachmentTypeRepository.EditAsync(model);

                return new ReturnResult<AttachmentTypeDto>(true, HttpStatuses.Status200OK, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<AttachmentTypeDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> RemoveAsync(int id)
        {
            try
            {
                await _attachmentTypeRepository.RemoveAsync(id);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, true);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<bool>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "لا يمكن حذف العنصر لارتباطه بعناصر اخرى." }
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

        public async Task<ReturnResult<bool>> IsNameExistsAsync(AttachmentTypeDto model)
        {
            try
            {
                var errors = new List<string>();

                bool result = await _attachmentTypeRepository.IsNameExistsAsync(model);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model.Id);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }
    }
}
