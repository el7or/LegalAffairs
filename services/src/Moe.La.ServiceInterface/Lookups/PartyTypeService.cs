//using AutoMapper;
//using Microsoft.Extensions.Logging;
//using Moe.La.Core.Dtos;
//using Moe.La.Core.Entities;
//using Moe.La.Core.Enums;
//using Moe.La.Core.Interfaces.Repositories;
//using Moe.La.Core.Interfaces.Services;
//using Moe.La.Core.Models;
//using Moe.La.ServiceInterface.Validators.Lookups;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace Moe.La.ServiceInterface
//{
//    public class PartyTypeService : IPartyTypeService
//    {
//        private readonly IPartyTypeRepository _partyTypeRepository;
//        private readonly ILogger<PartyTypeService> _logger;
//        private readonly IMapper _mapperConfig;
//        public PartyTypeService(IPartyTypeRepository PartyTypeRepository, IMapper mapperConfig, ILogger<PartyTypeService> logger)
//        {
//            _partyTypeRepository = PartyTypeRepository;
//            _logger = logger;
//            _mapperConfig = mapperConfig;
//        }

//        public async Task<ReturnResult<QueryResultDto<PartyTypeListItemDto>>> GetAllAsync(QueryObject queryObject)
//        {
//            try
//            {
//                var entities = await _partyTypeRepository.GetAllAsync(queryObject);

//                return new ReturnResult<QueryResultDto<PartyTypeListItemDto>>(true, HttpStatuses.Status200OK, entities);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, ex.Message, queryObject);

//                return new ReturnResult<QueryResultDto<PartyTypeListItemDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
//            }
//        }

//        public async Task<ReturnResult<PartyTypeListItemDto>> GetAsync(int id)
//        {
//            try
//            {
//                var entity = await _partyTypeRepository.GetAsync(id);

//                return new ReturnResult<PartyTypeListItemDto>(true, HttpStatuses.Status200OK, entity);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, ex.Message, id);

//                return new ReturnResult<PartyTypeListItemDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
//            }
//        }

//        public async Task<ReturnResult<PartyTypeDto>> AddAsync(PartyTypeDto model)
//        {
//            try
//            {
//                var validationResult = ValidationResult.CheckModelValidation(new PartyTypeValidator(), model);

//                if (!validationResult.IsValid)
//                {
//                    return new ReturnResult<PartyTypeDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
//                }

//                await _partyTypeRepository.AddAsync(model);

//                return new ReturnResult<PartyTypeDto>(true, HttpStatuses.Status201Created, model);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, ex.Message, model);

//                return new ReturnResult<PartyTypeDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
//            }
//        }

//        public async Task<ReturnResult<PartyTypeDto>> EditAsync(PartyTypeDto model)
//        {
//            try
//            {
//                var validationResult = ValidationResult.CheckModelValidation(new PartyTypeValidator(), model);

//                if (!validationResult.IsValid)
//                {
//                    return new ReturnResult<PartyTypeDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
//                }

//                await _partyTypeRepository.EditAsync(model);

//                return new ReturnResult<PartyTypeDto>(true, HttpStatuses.Status200OK, model);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, ex.Message, model);

//                return new ReturnResult<PartyTypeDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
//            }
//        }

//        public async Task<ReturnResult<bool>> RemoveAsync(int id)
//        {
//            try
//            {
//                await _partyTypeRepository.RemoveAsync(id);

//                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, true);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, ex.Message, id);

//                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
//            }
//        }
//    }
//}
