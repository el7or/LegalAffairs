using Microsoft.Extensions.Logging;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using Moe.La.Core.Models.Integration.Moeen;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class MoeenInformLetterIntegrationService : IMoeenInformLetterIntegrationService
    {
        private readonly IMoeenInformLetterRepository _moeenInformLetterRepository;
        private readonly ILogger<MoeenInformLetterIntegrationService> _logger;

        public MoeenInformLetterIntegrationService(IMoeenInformLetterRepository moeenInformLetterRepository, ILogger<MoeenInformLetterIntegrationService> logger)
        {
            _moeenInformLetterRepository = moeenInformLetterRepository;
            _logger = logger;
        }

        public async Task<ReturnResult<bool>> AddAsync(InformLetterInfoStructureModel informLetter)
        {
            try
            {
                await _moeenInformLetterRepository.AddAsync(informLetter);
                return new ReturnResult<bool>(true, HttpStatuses.Status201Created, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, informLetter);
                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }
    }
}
