using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class RequestLetterService : IRequestLetterService
    {
        private readonly IRequestLetterRepository _requestLetterRepository;
        private readonly ICaseSupportingDocumentRequestService _caseSupportingDocumentRequestService;
        private readonly ICaseService _caseService;
        private readonly IRequestService _requestService;

        private readonly ILogger<RequestLetterService> _logger;

        public RequestLetterService(IRequestLetterRepository requestLetterRepository, ICaseSupportingDocumentRequestService caseSupportingDocumentRequestService, ICaseService caseService, IRequestService requestService, ILogger<RequestLetterService> logger)
        {
            _requestLetterRepository = requestLetterRepository;
            _logger = logger;
            _caseSupportingDocumentRequestService = caseSupportingDocumentRequestService;
            _caseService = caseService;
            _requestService = requestService;

        }

        public async Task<ReturnResult<QueryResultDto<RequestLetterDto>>> GetAllAsync(QueryObject queryObject)
        {
            try
            {
                var entities = await _requestLetterRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<RequestLetterDto>>(true, HttpStatuses.Status200OK, entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, queryObject);

                return new ReturnResult<QueryResultDto<RequestLetterDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<RequestLetterDto>> GetAsync(int id)
        {
            try
            {
                var entitiy = await _requestLetterRepository.GetAsync(id);

                return new ReturnResult<RequestLetterDto>(true, HttpStatuses.Status200OK, entitiy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<RequestLetterDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<RequestLetterDto>> AddAsync(RequestLetterDto model)
        {
            try
            {
                await _requestService.UpdateStatusAsync(model.RequestId, model.RequestStatus);

                await _requestLetterRepository.AddAsync(model);

                // add request transaction
                if (model.RequestStatus == RequestStatuses.Modified)
                    await _requestService.AddTransactionAsync(new RequestTransactionDto { RequestId = model.RequestId, Description = "تم تعديل صياغة الطلب", TransactionType = RequestTransactionTypes.Modified });


                return new ReturnResult<RequestLetterDto>(true, HttpStatuses.Status201Created, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<RequestLetterDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<RequestLetterDto>> EditAsync(RequestLetterDto model)
        {
            try
            {
                await _requestService.UpdateStatusAsync(model.RequestId, model.RequestStatus);

                await _requestLetterRepository.EditAsync(model);

                // add request transaction
                if (model.RequestStatus == RequestStatuses.Modified)
                    await _requestService.AddTransactionAsync(new RequestTransactionDto { RequestId = model.RequestId, Description = "تم تعديل صياغة الطلب", TransactionType = RequestTransactionTypes.Modified });


                return new ReturnResult<RequestLetterDto>(true, HttpStatuses.Status201Created, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<RequestLetterDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> RemoveAsync(int id)
        {
            try
            {
                await _requestLetterRepository.RemoveAsync(id);

                return new ReturnResult<bool>(true, HttpStatuses.Status201Created, true);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<bool>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "لا يمكن حذف العنصر لارتباطه بعناصر اخرى." }
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

        public async Task<ReturnResult<RequestLetterDto>> GetByRequestIdAsync(int id)
        {
            try
            {
                var entitiy = await _requestLetterRepository.GetByRequestIdAsync(id);

                return new ReturnResult<RequestLetterDto>(true, HttpStatuses.Status200OK, entitiy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<RequestLetterDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<string>> ReplaceDocumentRequestContent(int requestID, int templateID)
        {
            try
            {
                var request = await _caseSupportingDocumentRequestService.GetAsync(requestID);
                var _case = await _caseService.GetAsync(request.Data.CaseId.Value);
                var content = await _requestLetterRepository.ReplaceDocumentRequestContent(templateID, _case.Data, request.Data);


                return new ReturnResult<string>(true, HttpStatuses.Status200OK, content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, requestID);

                return new ReturnResult<string>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<string>> ReplaceCaseCloseRequestContent(int caseId, int templateId)
        {
            try
            {
                var _case = await _caseService.GetAsync(caseId);
                var content = await _requestLetterRepository.ReplaceCaseCloseRequestContent(templateId, _case.Data);

                return new ReturnResult<string>(true, HttpStatuses.Status200OK, content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, templateId);

                return new ReturnResult<string>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public ReturnResult<string> ReplaceDeplartment(string contnet, string departmentName)
        {
            try
            {
                string matchCodeTag = @"\<span id='DepartmentName'>(.*?)\</span\>";
                string output = Regex.Replace(contnet, matchCodeTag, "<span id='DepartmentName'>" + departmentName + "</span>");
                return new ReturnResult<string>(true, HttpStatuses.Status200OK, output);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, departmentName);

                return new ReturnResult<string>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }

        }
    }
}
