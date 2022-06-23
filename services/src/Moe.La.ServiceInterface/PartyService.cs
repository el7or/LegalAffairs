using Microsoft.Extensions.Logging;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using Moe.La.ServiceInterface.Validators.Others;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class PartyService : IPartyService
    {
        private readonly IPartyRepository _partyRepository;
        private readonly ILogger<PartyService> _logger;

        public PartyService(IPartyRepository partyRepository, ILogger<PartyService> logger)
        {
            _partyRepository = partyRepository;
            _logger = logger;
        }

        public async Task<ReturnResult<QueryResultDto<PartyListItemDto>>> GetAllAsync(PartyQueryObject queryObject)
        {
            try
            {
                var entities = await _partyRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<PartyListItemDto>>(true, HttpStatuses.Status200OK, entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, queryObject);

                return new ReturnResult<QueryResultDto<PartyListItemDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<PartyDetailsDto>> GetAsync(int id)
        {
            try
            {
                var party = await _partyRepository.GetAsync(id);

                if (party is null)
                {
                    return new ReturnResult<PartyDetailsDto>(false, HttpStatuses.Status404NotFound, new List<string> { "البيانات المطلوبة غير متوفرة." });
                }

                return new ReturnResult<PartyDetailsDto>(true, HttpStatuses.Status200OK, party);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<PartyDetailsDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<PartyDto>> AddAsync(PartyDto model)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new PartyValidator(), model);
                if (!validationResult.IsValid)
                {
                    return new ReturnResult<PartyDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = validationResult.Errors,
                        Data = null
                    };
                }

                await _partyRepository.AddAsync(model);

                return new ReturnResult<PartyDto>(true, HttpStatuses.Status200OK, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<PartyDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<PartyDto>> EditAsync(PartyDto model)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new PartyValidator(), model);
                if (!validationResult.IsValid)
                {
                    return new ReturnResult<PartyDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = validationResult.Errors,
                        Data = null
                    };
                }

                await _partyRepository.EditAsync(model);

                return new ReturnResult<PartyDto>(true, HttpStatuses.Status200OK, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<PartyDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> RemoveAsync(int id)
        {
            try
            {
                await _partyRepository.RemoveAsync(id);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> IsPartyExist(PartyDto party)
        {
            try
            {
                bool result = await _partyRepository.IsPartyExist(party);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, party);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }

        }
    }
}
