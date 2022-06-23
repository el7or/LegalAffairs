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
    public class MoamalatRaselService : IMoamalatRaselService
    {
        private readonly IMoamalaRaselRepository _moamalaRaselRepository;
        private readonly IMoamalaService _moamalaService;
        private readonly IMinistryDepartmentService _ministryDepartmentService;
        private readonly IWorkItemTypeService _workItemTypeService;
        private readonly ILogger<MoamalatRaselService> _logger;

        public MoamalatRaselService(IMoamalaRaselRepository moamalaRaselRepository, IMoamalaService moamalaService, IWorkItemTypeService workItemTypeService,
            IMinistryDepartmentService ministryDepartmentService, ILogger<MoamalatRaselService> logger)
        {
            _moamalaRaselRepository = moamalaRaselRepository;
            _moamalaService = moamalaService;
            _ministryDepartmentService = ministryDepartmentService;
            _workItemTypeService = workItemTypeService;

            _logger = logger;
        }
        public async Task<ReturnResult<QueryResultDto<MoamalaRaselListItemDto>>> GetAllAsync(MoamalatRaselQueryObject queryObject)
        {
            try
            {
                var entities = await _moamalaRaselRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<MoamalaRaselListItemDto>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entities
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, queryObject);

                return new ReturnResult<QueryResultDto<MoamalaRaselListItemDto>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<MoamalaRaselDto>> GetAsync(int id)
        {
            try
            {
                var entitiy = await _moamalaRaselRepository.GetAsync(id);

                return new ReturnResult<MoamalaRaselDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entitiy
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<MoamalaRaselDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }
        public async Task<ReturnResult<MoamalaRaselDto>> AddAsync(MoamalaRaselDto moamalaRaselDto)
        {
            try
            {
                await _moamalaRaselRepository.AddAsync(moamalaRaselDto);

                return new ReturnResult<MoamalaRaselDto>(true, HttpStatuses.Status201Created, moamalaRaselDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, moamalaRaselDto);

                return new ReturnResult<MoamalaRaselDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });

            }
        }

        public async Task<ReturnResult<MoamalaRaselDto>> ReceiveAsync(int moamalaRaselId)
        {
            try
            {
                var moamalaRaselToAdd = await _moamalaRaselRepository.ReceiveMoamalaAsync(moamalaRaselId);

                //Check if department exist in  lookup and get the id and if not exist add  new department.              
                var departmentID = _ministryDepartmentService.GetDepartmentIdAsync(moamalaRaselToAdd.GroupNameTo).Result.Data;
                if (departmentID == 0)
                {
                    var department = _ministryDepartmentService.AddAsync(new MinistryDepartmentDto
                    {
                        Name = moamalaRaselToAdd.GroupNameTo
                    });
                    departmentID = department.Result.Data.Id;
                }
                //var workItemTypeID = _workItemTypeService.GetByNameAsync(moamalaRaselToAdd.ItemTypeName).Result.Data;
                //if (workItemTypeID== 0 )
                //{
                //    var workItemType = _workItemTypeService.AddAsync(new WorkItemTypeDto
                //    {
                //        Name = moamalaRaselToAdd.ItemTypeName
                //    });
                //    workItemTypeID = workItemType.Result.Data.Id.Value;
                //}
                var moamalaDto = new MoamalaDto()
                {
                    Subject = moamalaRaselToAdd.Subject,
                    Description = moamalaRaselToAdd.Comments,
                    SenderDepartmentId = departmentID,
                    //GeneralManagementId = moamalaRaselToAdd.GeneralManagementId,
                    UnifiedNo = moamalaRaselToAdd.UnifiedNumber,
                    MoamalaNumber = moamalaRaselToAdd.ItemNumber.ToString(),
                    ConfidentialDegree = (ConfidentialDegrees)moamalaRaselToAdd.ItemPrivacy,
                    Status = MoamalaStatuses.New,
                    PassType = PassTypes.Import,
                    PassDate = moamalaRaselToAdd.GregorianCreatedDate,
                    IsRead = false,
                    IsManual = false
                    //RelatedId= moamalaRaselToAdd.PreviousItemNumber,
                    // WorkItemTypeId= workItemTypeID,
                };

                await _moamalaService.AddAsync(moamalaDto);
                return new ReturnResult<MoamalaRaselDto>(true, HttpStatuses.Status201Created, moamalaRaselToAdd);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<MoamalaRaselDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });

            }
        }

        public async Task<ReturnResult<bool>> RemoveAsync(int id)
        {
            try
            {
                await _moamalaRaselRepository.RemoveAsync(id);

                return new ReturnResult<bool>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<bool>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

    }
}
