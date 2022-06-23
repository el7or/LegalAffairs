using Microsoft.Extensions.Logging;
using Moe.La.Core.Dtos;
using Moe.La.Core.Dtos.Requests;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using Moe.La.ServiceInterface.Validators.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class RequestMoamalatService : IRequestMoamalatService
    {
        private readonly IRequestMoamalatRepository _requestMoamalatRepository;
        private readonly IRequestService _requestService;
        private readonly IMoamalaService _moamalaService;
        private readonly ILogger<RequestMoamalatService> _logger;

        public RequestMoamalatService(IRequestMoamalatRepository requestMoamalatRepository, IRequestService requestService,
            IMoamalaService moamalaService,
            ILogger<RequestMoamalatService> logger)
        {
            _requestMoamalatRepository = requestMoamalatRepository;
            _requestService = requestService;
            _moamalaService = moamalaService;
            _logger = logger;
        }

        public async Task<ReturnResult<RequestMoamalatDto>> AddAsync(RequestMoamalatDto model)
        {
            try
            {
                var errors = new List<string>();

                //var validationResult = ValidationResult.CheckModelValidation(new RequestMoamalatValidator(), model);

                //if (!validationResult.IsValid)
                //{
                //    errors.AddRange(validationResult.Errors);
                //}

                var dbRequest = await _requestService.GetAsync((int)model.RequestId);

                if (dbRequest.Data == null)
                {
                    errors.Add("الطلب غير موجود");
                }

                if (errors.Any())
                {
                    return new ReturnResult<RequestMoamalatDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = errors
                    };
                }
                await _requestMoamalatRepository.AddAsync(model);

                return new ReturnResult<RequestMoamalatDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<RequestMoamalatDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<ExportRequestDto>> ExportRequest(ExportRequestDto exportRequestDto)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new ExportRequestValidator(), exportRequestDto);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<ExportRequestDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                // add moamala
                var moamala = new MoamalaDto
                {
                    Subject = "Export Request",
                    Description = "Export Request",
                    MoamalaNumber = exportRequestDto.MoamalaNo,
                    PassType = PassTypes.Export,
                    PassDate = exportRequestDto.MoamalaDate,
                    Status = MoamalaStatuses.New,
                    IsRead = false,
                    //DepartmentId = 1,  //nullable
                };

                var dbMoamala = _moamalaService.AddAsync(moamala);


                if (dbMoamala.Result.Data != null)
                {
                    //add request_moamala 
                    await AddAsync(new RequestMoamalatDto { RequestId = exportRequestDto.requestId, MoamalatId = (int)moamala.Id });
                }

                // change request status to exported
                //await _requestRepository.ChangeRequestStatus(exportRequestDto.requestId, RequestStatuses.Exported);

                // add request transaction
                var transactionToAdd = new RequestTransactionDto { RequestId = exportRequestDto.requestId };
                transactionToAdd.Description = "تم تصدير الطلب";
                transactionToAdd.TransactionType = RequestTransactionTypes.Exported;
                await _requestService.AddTransactionAsync(transactionToAdd);


                return new ReturnResult<ExportRequestDto>(true, HttpStatuses.Status200OK, exportRequestDto);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, exportRequestDto);

                return new ReturnResult<ExportRequestDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

    }
}
