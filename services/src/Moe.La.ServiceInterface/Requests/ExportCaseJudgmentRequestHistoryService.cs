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
    public class ExportCaseJudgmentRequestHistoryService : IExportCaseJudgmentRequestHistoryService
    {
        private readonly IExportCaseJudgmentRequestHistoryRepository _exportCaseJudgmentRequestHistoryRepository;
        private readonly ILogger<ExportCaseJudgmentRequestHistoryService> _logger;

        public ExportCaseJudgmentRequestHistoryService(IExportCaseJudgmentRequestHistoryRepository exportCaseJudgmentRequestHistoryRepository, ILogger<ExportCaseJudgmentRequestHistoryService> logger)
        {
            _logger = logger;
            _exportCaseJudgmentRequestHistoryRepository = exportCaseJudgmentRequestHistoryRepository;
        }

        public async Task<ReturnResult<ExportCaseJudgmentRequestHistoryListItemDto>> GetAsync(int id)
        {
            try
            {
                var entity = await _exportCaseJudgmentRequestHistoryRepository.GetAsync(id);

                return new ReturnResult<ExportCaseJudgmentRequestHistoryListItemDto>(true, HttpStatuses.Status201Created, entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<ExportCaseJudgmentRequestHistoryListItemDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }


        public async Task AddAsync(int Id)
        {
            try
            {
                await _exportCaseJudgmentRequestHistoryRepository.AddAsync(Id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, Id);

            }
        }

    }
}
