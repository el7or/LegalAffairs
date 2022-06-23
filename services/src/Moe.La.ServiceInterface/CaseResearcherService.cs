using Microsoft.Extensions.Logging;
using Moe.La.Core.Dtos;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class CaseResearcherService : ICaseResearcherService
    {
        private readonly ICaseResearchersRepository _caseResearchersRepository;
        private readonly ILogger<CaseResearcherService> _logger;

        public CaseResearcherService(ICaseResearchersRepository caseResearchersRepository, ILogger<CaseResearcherService> logger)
        {
            _caseResearchersRepository = caseResearchersRepository;
            _logger = logger;
        }

        public async Task<ReturnResult<CaseResearchersDto>> GetByCaseAsync(int id)
        {
            try
            {
                var entitiy = await _caseResearchersRepository.GetByCaseAsync(id);

                return new ReturnResult<CaseResearchersDto>(true, HttpStatuses.Status200OK, entitiy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<CaseResearchersDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<CaseResearchersDto>> GetByCaseAsync(int id, Guid? ResearcherId = null)
        {
            try
            {
                var entitiy = await _caseResearchersRepository.GetByCaseAsync(id, ResearcherId);

                return new ReturnResult<CaseResearchersDto>(true, HttpStatuses.Status200OK, entitiy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<CaseResearchersDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<CaseResearchersDto>> AddResearcher(CaseResearchersDto model)
        {
            try
            {
                await _caseResearchersRepository.AddResearcher(model);

                return new ReturnResult<CaseResearchersDto>(true, HttpStatuses.Status201Created, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<CaseResearchersDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<CaseResearchersDto>> AddIntegratedResearcher(CaseResearchersDto model)
        {
            try
            {
                await _caseResearchersRepository.AddIntegratedResearcher(model);

                return new ReturnResult<CaseResearchersDto>(true, HttpStatuses.Status201Created, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<CaseResearchersDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> RemoveAsync(int id)
        {
            try
            {
                await _caseResearchersRepository.RemoveAsync(id);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }
    }
}
