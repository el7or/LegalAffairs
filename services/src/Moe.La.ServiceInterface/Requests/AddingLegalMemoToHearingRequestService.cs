using Microsoft.Extensions.Logging;
using Moe.La.Core.Dtos;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using Moe.La.ServiceInterface.Validators.Request;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class AddingLegalMemoToHearingRequestService : IAddingLegalMemoToHearingRequestService
    {
        private readonly IAddingLegalMemoToHearingRequestRepository _hearingLegalMemoReviewRequestRepository;
        private readonly IHearingLegalMemoService _hearingLegalMemoService;
        private readonly IRequestService _requestService;
        private readonly IResearchsConsultantService _researcherConsultantService;
        private readonly ILogger<AddingLegalMemoToHearingRequestService> _logger;

        public AddingLegalMemoToHearingRequestService(IAddingLegalMemoToHearingRequestRepository hearingLegalMemoReviewRequestRepository, IRequestService requestService, IHearingLegalMemoService hearingLegalMemoService, ILogger<AddingLegalMemoToHearingRequestService> logger, IResearchsConsultantService researcherConsultantService)
        {
            _logger = logger;
            _hearingLegalMemoReviewRequestRepository = hearingLegalMemoReviewRequestRepository;
            _hearingLegalMemoService = hearingLegalMemoService;
            _requestService = requestService;
            _researcherConsultantService = researcherConsultantService;
        }

        public async Task<ReturnResult<AddingLegalMemoToHearingRequestDto>> AddAsync(AddingLegalMemoToHearingRequestDto hearingLegalMemoReviewRequestDto)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new AddingLegalMemoToHearingRequestValidator(), hearingLegalMemoReviewRequestDto);

                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }
                //check if hearing has any legal memos
                var hearingHasLegalMemo = await _hearingLegalMemoService.HearingHasLegalMomo(hearingLegalMemoReviewRequestDto.HearingId);
                if (hearingHasLegalMemo.Data)
                {
                    errors.Add("يوجد مذكرة سابقة مرتبطة بهذه الجلسة");
                }

                var researcherConsultant = await _researcherConsultantService.CheckCurrentResearcherHasConsultantAsync();

                if (!researcherConsultant.Data)
                {
                    errors.Add("لايوجد مستشار معين لهذا الباحث ");
                }

                if (errors.Count > 0)
                {
                    return new ReturnResult<AddingLegalMemoToHearingRequestDto>(false, HttpStatuses.Status400BadRequest, errors);
                }

                var request = await _hearingLegalMemoReviewRequestRepository.AddAsync(hearingLegalMemoReviewRequestDto);

                // add request transaction
                await _requestService.AddTransactionAsync(new RequestTransactionDto { RequestId = request.Id, Description = "", TransactionType = RequestTransactionTypes.AddLegalMemoToHearing });

                return new ReturnResult<AddingLegalMemoToHearingRequestDto>(true, HttpStatuses.Status201Created, request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, hearingLegalMemoReviewRequestDto);

                return new ReturnResult<AddingLegalMemoToHearingRequestDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<AddingLegalMemoToHearingRequestDetailsDto>> GetAsync(int Id)
        {
            try
            {
                var entity = await _hearingLegalMemoReviewRequestRepository.GetAsync(Id);

                return new ReturnResult<AddingLegalMemoToHearingRequestDetailsDto>(true, HttpStatuses.Status200OK, entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, Id);

                return new ReturnResult<AddingLegalMemoToHearingRequestDetailsDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }
        public async Task<ReturnResult<ReplyAddingLegalMemoToHearingRequestDto>> ReplyAddingMemoHearingRequest(ReplyAddingLegalMemoToHearingRequestDto replyAddingLegalMemoToHearingRequest)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new ReplyAddingLegalMemoToHearingRequestValidator(), replyAddingLegalMemoToHearingRequest);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<ReplyAddingLegalMemoToHearingRequestDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                // save the reply note and date and AddingHearingMemo request status
                var addingMemoToHearing = await _hearingLegalMemoReviewRequestRepository
                     .ReplyAddingMemoHearingRequestAsync(replyAddingLegalMemoToHearingRequest);

                if (replyAddingLegalMemoToHearingRequest.RequestStatus == RequestStatuses.Accepted)
                {
                    await _hearingLegalMemoService.AddAsync(replyAddingLegalMemoToHearingRequest.Id);
                }

                // add request transaction
                var transactionToAdd = new RequestTransactionDto { RequestId = addingMemoToHearing.Id };
                //if (addingMemoToHearing.Request.RequestStatus == RequestStatuses.Returned)
                //{
                //    transactionToAdd.Description = addingMemoToHearing.ReplyNote;
                //    transactionToAdd.TransactionType = RequestTransactionTypes.Returned;
                //}
                if (addingMemoToHearing.Request.RequestStatus == RequestStatuses.Rejected)
                {
                    transactionToAdd.Description = addingMemoToHearing.ReplyNote;
                    transactionToAdd.TransactionType = RequestTransactionTypes.RequestAccepted;
                }
                if (addingMemoToHearing.Request.RequestStatus == RequestStatuses.Accepted)
                {
                    transactionToAdd.Description = "-";
                    transactionToAdd.TransactionType = RequestTransactionTypes.RequestAccepted;
                }
                await _requestService.AddTransactionAsync(transactionToAdd);
                return new ReturnResult<ReplyAddingLegalMemoToHearingRequestDto>(true, HttpStatuses.Status200OK, replyAddingLegalMemoToHearingRequest);

            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, replyAddingLegalMemoToHearingRequest);

                return new ReturnResult<ReplyAddingLegalMemoToHearingRequestDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }
    }
}
