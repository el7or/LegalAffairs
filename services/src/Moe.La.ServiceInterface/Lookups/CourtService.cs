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
    public class CourtService : ICourtService
    {
        private readonly ICourtRepository _CourtRepository;
        private readonly ILogger<CourtService> _logger;

        public CourtService(ICourtRepository CourtRepository, ILogger<CourtService> logger)
        {
            _CourtRepository = CourtRepository;
            _logger = logger;
        }

        public async Task<ReturnResult<QueryResultDto<CourtListItemDto>>> GetAllAsync(CourtQueryObject queryObject)
        {
            try
            {
                var entities = await _CourtRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<CourtListItemDto>>(true, HttpStatuses.Status200OK, entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, queryObject);

                return new ReturnResult<QueryResultDto<CourtListItemDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<CourtListItemDto>> GetAsync(int id)
        {
            try
            {
                var entitiy = await _CourtRepository.GetAsync(id);

                return new ReturnResult<CourtListItemDto>(true, HttpStatuses.Status200OK, entitiy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<CourtListItemDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<CourtDto>> AddAsync(CourtDto model)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new CourtValidator(), model);

                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                bool isCourtExists = await _CourtRepository.IsNameExistsAsync(model);
                if (isCourtExists)
                {
                    errors.Add(" اسم المحكمة موجود في نفس الفئة والدرجة.");
                }

                if (errors.Any())
                    return new ReturnResult<CourtDto>(false, HttpStatuses.Status400BadRequest, errors);

                await _CourtRepository.AddAsync(model);

                return new ReturnResult<CourtDto>(true, HttpStatuses.Status201Created, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<CourtDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<CourtDto>> EditAsync(CourtDto model)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new CourtValidator(), model);

                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                bool isCourtExists = await _CourtRepository.IsNameExistsAsync(model);
                if (isCourtExists)
                {
                    errors.Add("اسم المحكمة موجود في نفس الفئة والدرجة.");
                }

                if (errors.Any())
                    return new ReturnResult<CourtDto>(false, HttpStatuses.Status400BadRequest, errors);

                await _CourtRepository.EditAsync(model);

                return new ReturnResult<CourtDto>(true, HttpStatuses.Status200OK, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<CourtDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<int>> RemoveAsync(int id)
        {
            try
            {
                await _CourtRepository.RemoveAsync(id);

                return new ReturnResult<int>(true, HttpStatuses.Status200OK, id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<int>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "لا يمكن حذف العنصر لارتباطه بعناصر اخرى." }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<int>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<bool>> IsNameExistsAsync(CourtDto courtDto)
        {
            try
            {
                var errors = new List<string>();

                bool result = await _CourtRepository.IsNameExistsAsync(courtDto);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, courtDto.Id);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }
    }
}
