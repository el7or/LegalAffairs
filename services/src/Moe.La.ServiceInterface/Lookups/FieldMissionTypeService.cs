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
//    public class FieldMissionTypeService : IFieldMissionTypeService
//    {
//        private readonly IFieldMissionTypeRepository _fieldMissionTypeRepository;
//        private readonly ILogger<FieldMissionTypeService> _logger;

//        public FieldMissionTypeService(IFieldMissionTypeRepository FieldMissionTypeRepository, ILogger<FieldMissionTypeService> logger)
//        {
//            _fieldMissionTypeRepository = FieldMissionTypeRepository;
//            _logger = logger;
//        }

//        public async Task<ReturnResult<QueryResultDto<FieldMissionTypeListItemDto>>> GetAllAsync(QueryObject queryObject)
//        {
//            try
//            {
//                var entities = await _fieldMissionTypeRepository.GetAllAsync(queryObject);

//                return new ReturnResult<QueryResultDto<FieldMissionTypeListItemDto>>(true, HttpStatuses.Status200OK, entities);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, ex.Message, queryObject);

//                return new ReturnResult<QueryResultDto<FieldMissionTypeListItemDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
//            }
//        }

//        public async Task<ReturnResult<FieldMissionTypeListItemDto>> GetAsync(int id)
//        {
//            try
//            {
//                var entitiy = await _fieldMissionTypeRepository.GetAsync(id);

//                return new ReturnResult<FieldMissionTypeListItemDto>(true, HttpStatuses.Status200OK, entitiy);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, ex.Message, id);

//                return new ReturnResult<FieldMissionTypeListItemDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
//            }
//        }

//        public async Task<ReturnResult<FieldMissionTypeDto>> AddAsync(FieldMissionTypeDto model)
//        {
//            try
//            {
//                var validationResult = ValidationResult.CheckModelValidation(new FieldMissionTypeValidator(), model);

//                if (!validationResult.IsValid)
//                {
//                    return new ReturnResult<FieldMissionTypeDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
//                }

//                await _fieldMissionTypeRepository.AddAsync(model);

//                return new ReturnResult<FieldMissionTypeDto>(true, HttpStatuses.Status201Created, model);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, ex.Message, model);

//                return new ReturnResult<FieldMissionTypeDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
//            }
//        }

//        public async Task<ReturnResult<FieldMissionTypeDto>> EditAsync(FieldMissionTypeDto model)
//        {
//            try
//            {
//                var validationResult = ValidationResult.CheckModelValidation(new FieldMissionTypeValidator(), model);

//                if (!validationResult.IsValid)
//                {
//                    return new ReturnResult<FieldMissionTypeDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
//                }

//                await _fieldMissionTypeRepository.EditAsync(model);

//                return new ReturnResult<FieldMissionTypeDto>(true, HttpStatuses.Status200OK, model);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, ex.Message, model);

//                return new ReturnResult<FieldMissionTypeDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
//            }
//        }

//        public async Task<ReturnResult<bool>> RemoveAsync(int id)
//        {
//            try
//            {
//                await _fieldMissionTypeRepository.RemoveAsync(id);

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
