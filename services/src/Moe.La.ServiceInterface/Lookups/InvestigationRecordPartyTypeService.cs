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
    public class InvestigationRecordPartyTypeService : IInvestigationRecordPartyTypeService
    {
        private readonly IInvestigationRecordPartyTypeRepository _investigationRecordPartyTypeRepository;
        private readonly ILogger<InvestigationRecordPartyTypeService> _logger;

        public InvestigationRecordPartyTypeService(IInvestigationRecordPartyTypeRepository investigationRecordPartyTypeRepository, ILogger<InvestigationRecordPartyTypeService> logger)
        {
            _investigationRecordPartyTypeRepository = investigationRecordPartyTypeRepository;
            _logger = logger;
        }

        public async Task<ReturnResult<QueryResultDto<InvestigationRecordPartyTypeListItemDto>>> GetAllAsync(QueryObject queryObject)
        {
            try
            {
                var entities = await _investigationRecordPartyTypeRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<InvestigationRecordPartyTypeListItemDto>>(true, HttpStatuses.Status200OK, entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, queryObject);

                return new ReturnResult<QueryResultDto<InvestigationRecordPartyTypeListItemDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<InvestigationRecordPartyTypeListItemDto>> GetAsync(int id)
        {
            try
            {
                var entitiy = await _investigationRecordPartyTypeRepository.GetAsync(id);

                return new ReturnResult<InvestigationRecordPartyTypeListItemDto>(true, HttpStatuses.Status200OK, entitiy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<InvestigationRecordPartyTypeListItemDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<InvestigationRecordPartyTypeDto>> AddAsync(InvestigationRecordPartyTypeDto model)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new InvestigationRecordPartyTypeValidator(), model);

                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                bool isNameExists = await _investigationRecordPartyTypeRepository.IsNameExistsAsync(model.Name);
                if (isNameExists)
                {
                    errors.Add("اسم نوع الطرف موجود مسبقاً.");
                }

                if (errors.Any())
                {
                    return new ReturnResult<InvestigationRecordPartyTypeDto>(false, HttpStatuses.Status400BadRequest, errors);
                }

                await _investigationRecordPartyTypeRepository.AddAsync(model);

                return new ReturnResult<InvestigationRecordPartyTypeDto>(true, HttpStatuses.Status201Created, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<InvestigationRecordPartyTypeDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<InvestigationRecordPartyTypeDto>> EditAsync(InvestigationRecordPartyTypeDto model)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new InvestigationRecordPartyTypeValidator(), model);

                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                bool isNameExists = await _investigationRecordPartyTypeRepository.IsNameExistsAsync(model.Name);
                if (isNameExists)
                {
                    errors.Add("اسم نوع الطرف موجود مسبقاً.");
                }

                if (errors.Any())
                {
                    return new ReturnResult<InvestigationRecordPartyTypeDto>(false, HttpStatuses.Status400BadRequest, errors);
                }

                await _investigationRecordPartyTypeRepository.EditAsync(model);

                return new ReturnResult<InvestigationRecordPartyTypeDto>(true, HttpStatuses.Status200OK, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<InvestigationRecordPartyTypeDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> RemoveAsync(int id)
        {
            try
            {
                await _investigationRecordPartyTypeRepository.RemoveAsync(id);

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

        public async Task<ReturnResult<bool>> IsNameExistsAsync(string name)
        {
            try
            {
                var errors = new List<string>();

                bool result = await _investigationRecordPartyTypeRepository.IsNameExistsAsync(name);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, name);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }
    }
}
