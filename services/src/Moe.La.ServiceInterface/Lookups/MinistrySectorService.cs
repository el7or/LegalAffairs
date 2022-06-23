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
    public class MinistrySectorService : IMinistrySectorService
    {
        private readonly IMinistrySectorRepository _ministrySectorRepository;
        private readonly ILogger<MinistrySectorService> _logger;

        public MinistrySectorService(IMinistrySectorRepository MinistrySectorRepository, ILogger<MinistrySectorService> logger)
        {
            _ministrySectorRepository = MinistrySectorRepository;
            _logger = logger;
        }

        public async Task<ReturnResult<QueryResultDto<MinistrySectorListItemDto>>> GetAllAsync(QueryObject queryObject)
        {
            try
            {
                var entities = await _ministrySectorRepository.GetAllAsync(queryObject);

                if (entities == null)
                {
                    return new ReturnResult<QueryResultDto<MinistrySectorListItemDto>>(false, HttpStatuses.Status404NotFound, new List<string> { "البيانات المطلوبة غير متوفرة" });
                }

                return new ReturnResult<QueryResultDto<MinistrySectorListItemDto>>(true, HttpStatuses.Status200OK, entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, queryObject);

                return new ReturnResult<QueryResultDto<MinistrySectorListItemDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<MinistrySectorListItemDto>> GetAsync(int id)
        {
            try
            {
                var entitiy = await _ministrySectorRepository.GetAsync(id);

                return new ReturnResult<MinistrySectorListItemDto>(true, HttpStatuses.Status200OK, entitiy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<MinistrySectorListItemDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<MinistrySectorDto>> AddAsync(MinistrySectorDto model)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new MinistrySectorValidator(), model);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                bool isMinistrySectorExists = await _ministrySectorRepository.IsNameExistsAsync(model.Name);
                if (isMinistrySectorExists)
                {
                    errors.Add("الادارة موجودة مسبقاً");
                }

                if (errors.Any())
                {
                    return new ReturnResult<MinistrySectorDto>(false, HttpStatuses.Status400BadRequest, errors);
                }

                await _ministrySectorRepository.AddAsync(model);

                return new ReturnResult<MinistrySectorDto>(true, HttpStatuses.Status201Created, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<MinistrySectorDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<MinistrySectorDto>> EditAsync(MinistrySectorDto model)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new MinistrySectorValidator(), model);

                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                bool isMinistrySectorExists = await _ministrySectorRepository.IsNameExistsAsync(model.Name);
                if (isMinistrySectorExists)
                {
                    errors.Add("الادارة موجودة مسبقاً");
                }

                if (errors.Any())
                {
                    return new ReturnResult<MinistrySectorDto>(false, HttpStatuses.Status400BadRequest, errors);
                }

                await _ministrySectorRepository.EditAsync(model);

                return new ReturnResult<MinistrySectorDto>(true, HttpStatuses.Status200OK, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<MinistrySectorDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> RemoveAsync(int id, Guid userId)
        {
            try
            {
                await _ministrySectorRepository.RemoveAsync(id, userId);

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

        public async Task<ReturnResult<int>> GetSectorIdAsync(string name)
        {
            try
            {
                var SectorId = await _ministrySectorRepository.GetSectorIdAsync(name);

                return new ReturnResult<int>(true, HttpStatuses.Status200OK, SectorId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, name);

                return new ReturnResult<int>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> IsNameExistsAsync(string name)
        {
            try
            {
                var errors = new List<string>();

                bool result = await _ministrySectorRepository.IsNameExistsAsync(name);

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
