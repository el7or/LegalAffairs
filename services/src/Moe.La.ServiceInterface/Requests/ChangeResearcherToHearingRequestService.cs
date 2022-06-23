using Microsoft.Extensions.Logging;
using Moe.La.Core.Constants;
using Moe.La.Core.Dtos;
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
    public class ChangeResearcherToHearingRequestService : IChangeResearcherToHearingRequestService
    {
        private readonly IChangeResearcherToHearingRequestRepository _changeResearcherToHearingRequestRepository;
        private readonly IHearingService _hearingService;
        private readonly IRequestService _requestService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        private readonly ILogger<ChangeResearcherToHearingRequestService> _logger;

        public ChangeResearcherToHearingRequestService(
            IChangeResearcherToHearingRequestRepository changeResearcherToHearingRequestRepository,
            IHearingService hearingService,
            IRequestService requestService,
            IUserService userService,
            INotificationService notificationService,
            ILogger<ChangeResearcherToHearingRequestService> logger)
        {
            _changeResearcherToHearingRequestRepository = changeResearcherToHearingRequestRepository;
            _hearingService = hearingService;
            _requestService = requestService;
            _userService = userService;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task<ReturnResult<ChangeResearcherToHearingRequestListItemDto>> GetAsync(int id)
        {
            try
            {
                var entitiy = await _changeResearcherToHearingRequestRepository.GetAsync(id);

                if (entitiy == null)
                {
                    return new ReturnResult<ChangeResearcherToHearingRequestListItemDto>(false, HttpStatuses.Status404NotFound, new List<string> { "البيانات المطلوبة غير متوفرة" });
                }

                return new ReturnResult<ChangeResearcherToHearingRequestListItemDto>(true, HttpStatuses.Status200OK, entitiy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<ChangeResearcherToHearingRequestListItemDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<ChangeResearcherToHearingRequestDetailsDto>> AddAsync(ChangeResearcherToHearingRequestDto changeResearcherToHearingRequestDto)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new ChangeResearcherToHearingRequestValidator(), changeResearcherToHearingRequestDto);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                bool isThereMoreRequestForSameHearing = await _changeResearcherToHearingRequestRepository.IsMoreRequestsForSameHearing(changeResearcherToHearingRequestDto);
                if (isThereMoreRequestForSameHearing)
                {
                    errors.Add("لا يسمح بتقديم اكثر من طلب لنفس الجلسة.. مدير الترافع لم يرد علي الطلب السابق بعد.");
                }

                if (errors.Any())
                    return new ReturnResult<ChangeResearcherToHearingRequestDetailsDto>(false, HttpStatuses.Status400BadRequest, errors);

                var changeResearcherRequest = await _changeResearcherToHearingRequestRepository.AddAsync(changeResearcherToHearingRequestDto);

                // Add Transaction
                var requestTransactionDto = new RequestTransactionDto()
                {
                    RequestId = changeResearcherRequest.Id,
                    RequestStatus = RequestStatuses.New,
                    TransactionType = RequestTransactionTypes.Create,
                    Description = ""
                };

                await _requestService.AddTransactionAsync(requestTransactionDto);

                await NotifyUsersWithAddRequest(changeResearcherRequest);

                return new ReturnResult<ChangeResearcherToHearingRequestDetailsDto>(true, HttpStatuses.Status201Created, changeResearcherRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, changeResearcherToHearingRequestDto);

                return new ReturnResult<ChangeResearcherToHearingRequestDetailsDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<ReplyChangeResearcherToHearingRequestDto>> AcceptChangeResearcherToHearingRequest(ReplyChangeResearcherToHearingRequestDto replyChangeResearcherToHearingRequestDto)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new ReplyChangeResearcherToHearingRequestValidator(), replyChangeResearcherToHearingRequestDto);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<ReplyChangeResearcherToHearingRequestDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                // if the current ResearcherId and new ResearcherId are the same ==> throw exception
                if (replyChangeResearcherToHearingRequestDto.CurrentResearcherId == replyChangeResearcherToHearingRequestDto.NewResearcherId)
                {
                    return new ReturnResult<ReplyChangeResearcherToHearingRequestDto>(false, HttpStatuses.Status400BadRequest, new List<string> { "الباحث الجديد المختار هو نفسة الباحث الحالي، الرجاء اختيار باحث آخر." });
                }

                // save the reply note and date and change request status
                var changeResearcherToHearingRequestDto = await _changeResearcherToHearingRequestRepository
                     .ReplyChangeResearcherToHearingRequestAsync(replyChangeResearcherToHearingRequestDto, RequestStatuses.Accepted);

                // add the new researcher to hearing
                await _hearingService.AssignHearingToAsync(changeResearcherToHearingRequestDto.HearingId, replyChangeResearcherToHearingRequestDto.NewResearcherId.Value);

                // Add Transaction
                string description = "قبول طلب رقم <<" + changeResearcherToHearingRequestDto.Id + ">> تغيير الباحث من <<" + changeResearcherToHearingRequestDto.Request.CreatedBy.Name + ">> بصفته مكلف بحضور الجلسة رقم <<" + changeResearcherToHearingRequestDto.HearingId + ">> .";

                var requestTransactionDto = new RequestTransactionDto()
                {
                    RequestId = changeResearcherToHearingRequestDto.Id,
                    RequestStatus = RequestStatuses.Accepted,
                    TransactionType = RequestTransactionTypes.RequestAccepted,
                    Description = description
                };

                await _requestService.AddTransactionAsync(requestTransactionDto);

                await NotifyUsersWithAcceptRequest(replyChangeResearcherToHearingRequestDto);

                return new ReturnResult<ReplyChangeResearcherToHearingRequestDto>(true, HttpStatuses.Status200OK, replyChangeResearcherToHearingRequestDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, replyChangeResearcherToHearingRequestDto);

                return new ReturnResult<ReplyChangeResearcherToHearingRequestDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<ReplyChangeResearcherToHearingRequestDto>> RejectChangeResearcherToHearingRequest(ReplyChangeResearcherToHearingRequestDto replyChangeResearcherToHearingRequestDto)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new ReplyChangeResearcherToHearingRequestValidator(), replyChangeResearcherToHearingRequestDto);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<ReplyChangeResearcherToHearingRequestDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                // save the reply note and date and change request status
                var changeResearcherToHearingRequestDto = await _changeResearcherToHearingRequestRepository
                     .ReplyChangeResearcherToHearingRequestAsync(replyChangeResearcherToHearingRequestDto, RequestStatuses.Rejected);

                // Add Transaction
                string description = "رفض طلب رقم <<" + changeResearcherToHearingRequestDto.Id + ">> تغيير الباحث من <<" + changeResearcherToHearingRequestDto.Request.CreatedBy.Name + ">> بصفته مكلف بحضور الجلسة رقم <<" + changeResearcherToHearingRequestDto.HearingId + ">> .";

                var requestTransactionDto = new RequestTransactionDto()
                {
                    RequestId = changeResearcherToHearingRequestDto.Id,
                    RequestStatus = RequestStatuses.Rejected,
                    TransactionType = RequestTransactionTypes.RequestRejected,
                    Description = description
                };

                await _requestService.AddTransactionAsync(requestTransactionDto);

                return new ReturnResult<ReplyChangeResearcherToHearingRequestDto>(true, HttpStatuses.Status200OK, replyChangeResearcherToHearingRequestDto);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, replyChangeResearcherToHearingRequestDto);

                return new ReturnResult<ReplyChangeResearcherToHearingRequestDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<Task> CanceledChangeResearcherToHearingRequests()
        {
            try
            {
                _logger.LogInformation($"{nameof(CanceledChangeResearcherToHearingRequests)} job is starting...");

                await _changeResearcherToHearingRequestRepository.CanceledChangeResearcherToHearingRequests();

                _logger.LogInformation($"{nameof(CanceledChangeResearcherToHearingRequests)} job ended successfully.");

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Task.FromException(ex);
            }
        }

        public async Task NotifyUsersWithAddRequest(ChangeResearcherToHearingRequestDetailsDto changeResearcherToHearingRequestDto)
        {
            var litigationManagers = (await _userService.GetUsersByRolesAsync(ApplicationRolesConstants.DepartmentManager.Code, changeResearcherToHearingRequestDto.Request.ReceiverGeneralManagementId, (int)Departments.Litigation)).Data;

            // send a notification to litigation managers when add request
            var notificationDto = new NotificationDto()
            {
                Id = 0,
                Text = "تم تقديم طلب من الباحث " + changeResearcherToHearingRequestDto.Request.CreatedByFullName,
                URL = "/requests/view/" + changeResearcherToHearingRequestDto.Id + "/" + (int)RequestTypes.RequestResearcherChangeToHearing,
                SendEmailMessage = false,
                UserIds = litigationManagers.Select(m => m.Id).ToList()
            };

            await _notificationService.AddAsync(notificationDto);
        }

        public async Task NotifyUsersWithAcceptRequest(ReplyChangeResearcherToHearingRequestDto replyChangeResearcherToHearingRequest)
        {
            var userIds = new List<Guid>();

            // add current researcher to list
            userIds.Add(replyChangeResearcherToHearingRequest.CurrentResearcherId);

            // add new researcher to list
            userIds.Add(replyChangeResearcherToHearingRequest.NewResearcherId.Value);

            // send a notification to users when accept request
            var notificationDto = new NotificationDto()
            {
                Id = 0,
                Text = "تم قبول الطلب من الباحث رقم الطلب " + replyChangeResearcherToHearingRequest.Id,
                URL = "/requests/view/" + replyChangeResearcherToHearingRequest.Id + "/" + (int)RequestTypes.RequestResearcherChangeToHearing,
                UserIds = userIds
            };

            await _notificationService.AddAsync(notificationDto);
        }
    }
}
