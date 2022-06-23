using Microsoft.Extensions.Logging;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class InvestiationRecordPartyService : IInvestiationRecordPartyService
    {
        private readonly ILogger<InvestiationRecordPartyService> _logger;
        private readonly IInvestiationRecordPartyRepository _investiationRecordPartyRepository;
        public InvestiationRecordPartyService(IInvestiationRecordPartyRepository investiationRecordPartyRepository, ILogger<InvestiationRecordPartyService> logger)
        {
            _investiationRecordPartyRepository = investiationRecordPartyRepository;
            _logger = logger;
        }
        public async Task<ReturnResult<bool>> CheckPartyExistAsync(string identityNumber, int? investigationRecordId)
        {
            try
            {
                var entitiy = await _investiationRecordPartyRepository.CheckPartyExist(identityNumber, investigationRecordId);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, entitiy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, identityNumber);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }
    }
}

