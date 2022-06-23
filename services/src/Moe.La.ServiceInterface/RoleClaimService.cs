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
    public class RoleClaimService : IRoleClaimService
    {
        private readonly IRoleClaimRepository _roleClaimRepository;
        private readonly ILogger<RoleClaimService> _logger;

        public RoleClaimService(IRoleClaimRepository roleClaimRepository, ILogger<RoleClaimService> logger)
        {
            _roleClaimRepository = roleClaimRepository;
            _logger = logger;
        }

        public async Task<ReturnResult<QueryResultDto<RoleClaimListItemDto>>> GetAllAsync(QueryObject queryObject)
        {
            try
            {
                var entities = await _roleClaimRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<RoleClaimListItemDto>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entities
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, queryObject);

                return new ReturnResult<QueryResultDto<RoleClaimListItemDto>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

    }
}
