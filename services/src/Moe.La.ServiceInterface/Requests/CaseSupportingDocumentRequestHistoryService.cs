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
    public class CaseSupportingDocumentRequestHistoryService : ICaseSupportingDocumentRequestHistoryService
    {
        private readonly ICaseSupportingDocumentRequestHistoryRepository _caseSupportingDocumentRequestHistoryRepository;
        private readonly ILogger<CaseSupportingDocumentRequestHistoryService> _logger;

        public CaseSupportingDocumentRequestHistoryService(ICaseSupportingDocumentRequestHistoryRepository caseSupportingDocumentRequestHistoryRepository, ILogger<CaseSupportingDocumentRequestHistoryService> logger)
        {
            _logger = logger;
            _caseSupportingDocumentRequestHistoryRepository = caseSupportingDocumentRequestHistoryRepository;
        }

        public async Task<ReturnResult<CaseSupportingDocumentRequestHistoryListItemDto>> GetAsync(int id)
        {
            try
            {
                var entity = await _caseSupportingDocumentRequestHistoryRepository.GetAsync(id);

                return new ReturnResult<CaseSupportingDocumentRequestHistoryListItemDto>(true, HttpStatuses.Status201Created, entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<CaseSupportingDocumentRequestHistoryListItemDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }


        public async Task AddAsync(int Id)
        {
            try
            {
                await _caseSupportingDocumentRequestHistoryRepository.AddAsync(Id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, Id);

            }
        }

    }
}
