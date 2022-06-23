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
    public class HearingLegalMemoService : IHearingLegalMemoService
    {
        private readonly IHearingLegalMemoRepository _hearingLegalMemoRepository;

        private readonly ILogger<HearingLegalMemoService> _logger;
        public HearingLegalMemoService(IHearingLegalMemoRepository hearingLegalMemoRepository, ILogger<HearingLegalMemoService> logger)
        {
            _logger = logger;
            _hearingLegalMemoRepository = hearingLegalMemoRepository;
        }
        public async Task<ReturnResult<HearingLegalMemoDto>> AddAsync(int requestId)
        {
            try
            {
                var entity = await _hearingLegalMemoRepository.AddAsync(requestId);

                return new ReturnResult<HearingLegalMemoDto>(true, HttpStatuses.Status200OK, entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, requestId);

                return new ReturnResult<HearingLegalMemoDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }
        public async Task<ReturnResult<HearingLegalMemoDetailsDto>> GetByMemoAsync(int id)
        {
            try
            {
                var entity = await _hearingLegalMemoRepository.GetByMemoAsync(id);

                return new ReturnResult<HearingLegalMemoDetailsDto>(true, HttpStatuses.Status200OK, entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<HearingLegalMemoDetailsDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }
        public async Task<ReturnResult<bool>> HearingHasLegalMomo(int id)
        {
            try
            {
                var entity = await _hearingLegalMemoRepository.HearingHasLegalMomo(id);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

    }
}
