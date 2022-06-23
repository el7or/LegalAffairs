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
    public class InvestigationService : IInvestigationService
    {
        private readonly IInvestigationRepository _InvestigationRepository;
        private readonly ILogger<InvestigationService> _logger;

        public InvestigationService(IInvestigationRepository InvestigationRepository, ILogger<InvestigationService> logger)
        {
            _InvestigationRepository = InvestigationRepository;
            _logger = logger;
        }

        public async Task<ReturnResult<QueryResultDto<InvestigationListItemDto>>> GetAllAsync(InvestigationQueryObject queryObject)
        {
            try
            {
                var entities = await _InvestigationRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<InvestigationListItemDto>>(true, HttpStatuses.Status200OK, entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, queryObject);

                return new ReturnResult<QueryResultDto<InvestigationListItemDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<InvestigationDetailsDto>> GetAsync(int id)
        {
            try
            {
                var entitiy = await _InvestigationRepository.GetAsync(id);

                return new ReturnResult<InvestigationDetailsDto>(true, HttpStatuses.Status200OK, entitiy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<InvestigationDetailsDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<InvestigationDto>> AddAsync(InvestigationDto model)
        {
            try
            {
                await _InvestigationRepository.AddAsync(model);

                return new ReturnResult<InvestigationDto>(true, HttpStatuses.Status201Created, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<InvestigationDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<InvestigationDto>> EditAsync(InvestigationDto model)
        {
            try
            {
                await _InvestigationRepository.EditAsync(model);

                return new ReturnResult<InvestigationDto>(true, HttpStatuses.Status201Created, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<InvestigationDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> RemoveAsync(int id)
        {
            try
            {
                await _InvestigationRepository.RemoveAsync(id);

                return new ReturnResult<bool>(true, HttpStatuses.Status201Created, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }
    }
}
