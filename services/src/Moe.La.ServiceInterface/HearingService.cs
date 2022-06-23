using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moe.La.Common;
using Moe.La.Common.Extensions;
using Moe.La.Core.Constants;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using Moe.La.Integration;
using Moe.La.ServiceInterface.Validators.Others;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class HearingService : IHearingService
    {
        private readonly IHearingRepository _hearingRepository;
        private readonly ICaseService _caseService;
        private readonly IHearingUpdateService _hearingUpdateService;
        private readonly INotificationService _notificationService;
        private readonly IResearchsConsultantService _researchsConsultantService;
        private readonly ICaseResearcherService _caseResearcherService;
        private readonly IUserService _userService;
        private readonly IAttachmentService _attachmentService;
        private readonly AppSettings _appSettings;
        private readonly ILogger<HearingService> _logger;



        public HearingService(
            IHearingRepository hearingRepository,
            ICaseService caseService,
            IHearingUpdateService hearingUpdateService,
            INotificationService notificationService,
            IResearchsConsultantService researchsConsultantService,
            ICaseResearcherService caseResearcherService,
            IUserService userService,
            IAttachmentService attachmentService,
            IOptionsSnapshot<AppSettings> options,
            ILogger<HearingService> logger)
        {
            _hearingRepository = hearingRepository;
            _caseService = caseService;
            _hearingUpdateService = hearingUpdateService;
            _notificationService = notificationService;
            _researchsConsultantService = researchsConsultantService;
            _caseResearcherService = caseResearcherService;
            _userService = userService;
            _attachmentService = attachmentService;
            _logger = logger;
            _appSettings = options.Value;
        }

        public async Task<ReturnResult<QueryResultDto<HearingListItemDto>>> GetAllAsync(HearingQueryObject queryObject)
        {
            try
            {
                var entities = await _hearingRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<HearingListItemDto>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entities
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, queryObject);

                return new ReturnResult<QueryResultDto<HearingListItemDto>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string>() { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<HearingDetailsDto>> GetAsync(int id)
        {
            try
            {
                var entitiy = await _hearingRepository.GetAsync(id);

                return new ReturnResult<HearingDetailsDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entitiy
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<HearingDetailsDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string>() { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<HearingDto>> AddAsync(HearingDto model)
        {
            try
            {
                var errors = new List<string>();

                var validationResult = ValidationResult.CheckModelValidation(new HearingValidator(), model);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }
                var pronouncingJudgmentHearing = await _hearingRepository.GetCasePronouncingJudgmentHearing(model.Id, model.CaseId);

                if (pronouncingJudgmentHearing != null && model.Type == HearingTypes.PronouncingJudgment)
                {
                    errors.Add("تم إضافة جلسة النطق بالحكم بالفعل");

                    return new ReturnResult<HearingDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = errors
                    };
                }
                else if (pronouncingJudgmentHearing != null && model.HearingDate > pronouncingJudgmentHearing.HearingDate)
                {

                    errors.Add("غير مسموح بإضافة جلسة تالية لجلسة النطق بالحكم");

                    return new ReturnResult<HearingDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = errors
                    };
                }

                var isSheduledHearingExists = await _hearingRepository.IsCaseSchedulingHearingExists(model);
                if (isSheduledHearingExists)
                {
                    errors.Add("يوجد جلسة مجدولة بالفعل يجب ان تتحول الى منتهيه قبل إضافة جلسة مجدولة جديدة");
                }

                // bool firstHearing = await _hearingRepository.IsFirstHearingAsync(model);


                //// check if it is the first hearing ==> the user should be the researcher
                //if (!firstHearing)
                //{
                //    bool isCurrentUserResearcherToCase = await _hearingRepository.CheckUserAsync(model);

                //    if (isCurrentUserResearcherToCase)
                //        errors.Add("يتاح للباحث مستلم القضية إضافة بيانات اول جلسة فقط");
                //}

                //// we will check if there is another hearing for this case on the same date
                bool isHearingDateExists = await _hearingRepository.IsHearingDateExistsAsync(model);
                if (isHearingDateExists)
                {
                    errors.Add("توجد جلسة اخرى في نفس التاريخ");
                }


                if (model.Type == HearingTypes.PronouncingJudgment)
                {
                    bool isPleadingHearingExists = await _hearingRepository.CheckPleadingHearingsDate(model);

                    if (isPleadingHearingExists)
                        errors.Add("غير مسموح بإضافة جلسة نطق بالحكم بتاريخ سابق لجلسة مرافعة");
                }

                var caseDetails = await _caseService.GetAsync(model.CaseId);

                if (caseDetails.Data.StartDate > model.HearingDate)
                    errors.Add("غير مسموح بإضافة جلسة بتاريخ سابق لتاريخ بداية القضية");

                if (errors.Any())
                {
                    return new ReturnResult<HearingDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = errors
                    };
                }
                if (caseDetails.Data.Researchers.Count > 0)
                    model.AssignedToId = caseDetails.Data.Researchers.LastOrDefault().Id;
                await _hearingRepository.AddAsync(model);

                if (model.Attachments != null && model.Attachments.Count > 0)
                {
                    await _attachmentService.UpdateListAsync(model.Attachments);
                }


                await NotifyUsersWithNewHearingDate(model);

                //// to change case status
                //if (firstHearing ||
                //     (model.Type == HearingTypes.PronouncingJudgment && model.LitigationTypeId != null))
                //{
                //    CaseStatuses caseStatus = CaseStatuses.Circulation;
                //    if (model.Type == HearingTypes.PronouncingJudgment)
                //    {
                //        caseStatus = CaseStatuses.DoneJudgment;
                //    }

                //    var caseChangeStatusDto = new CaseChangeStatusDto()
                //    {
                //        Id = model.CaseId,
                //        Status = caseStatus,
                //        Note = string.Empty
                //    };

                //    await _caseRepository.ChangeStatusAsync(caseChangeStatusDto);
                //}

                return new ReturnResult<HearingDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<HearingDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string>() { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<HearingDto>> EditAsync(HearingDto model)
        {
            try
            {
                var errors = new List<string>();

                var validationResult = ValidationResult.CheckModelValidation(new HearingValidator(), model);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                //// edit hearing ////
                bool isHearingDateExists = await _hearingRepository.IsHearingDateExistsAsync(model);
                if (isHearingDateExists)
                {
                    errors.Add("توجد جلسة اخرى في نفس التاريخ");
                }
                var pronouncingJudgmentHearing = await _hearingRepository.GetCasePronouncingJudgmentHearing(model.Id, model.CaseId);
                if (pronouncingJudgmentHearing != null && model.Type == HearingTypes.PronouncingJudgment)
                {
                    errors.Add("تم إضافة جلسة النطق بالحكم بالفعل");
                }
                else if (pronouncingJudgmentHearing != null && model.HearingDate > pronouncingJudgmentHearing.HearingDate)
                {
                    errors.Add("غير مسموح بإضافة جلسة تالية لجلسة النطق بالحكم");
                }
                var isSheduledHearingExists = await _hearingRepository.IsCaseSchedulingHearingExists(model);
                if (isSheduledHearingExists)
                {
                    errors.Add("يوجد جلسة مجدولة بالفعل يجب ان تتحول الى منتهيه قبل إضافة جلسة مجدولة جديدة");
                }

                //// next new hearing ////
                if (model.WithNewHearing == true)
                {
                    if (model.HearingDate > model.NewHearing.HearingDate)
                    {
                        errors.Add("تاريخ الجلسة القادمة سابق لتاريخ الجلسة الحالية");
                    }

                    //// we will check if there is another hearing for this case on the same date
                    bool isNextHearingDateExists = await _hearingRepository.IsHearingDateExistsAsync(model.NewHearing);
                    if (isNextHearingDateExists)
                    {
                        errors.Add("توجد جلسة اخرى في نفس تاريخ الجلسة القادمة");
                    }

                    var pronouncingJudgmentNextHearing = await _hearingRepository.GetCasePronouncingJudgmentHearing(model.NewHearing.Id, model.NewHearing.CaseId);
                    if (pronouncingJudgmentNextHearing != null && model.NewHearing.Type == HearingTypes.PronouncingJudgment)
                    {
                        errors.Add("تم إضافة جلسة النطق بالحكم بالفعل");
                    }
                    else if (pronouncingJudgmentNextHearing != null && model.NewHearing.HearingDate > pronouncingJudgmentNextHearing.HearingDate)
                    {
                        errors.Add("غير مسموح بإضافة جلسة تالية لجلسة النطق بالحكم");
                    }

                    if (model.NewHearing.Type == HearingTypes.PronouncingJudgment)
                    {
                        bool isPleadingHearingExists = await _hearingRepository.CheckPleadingHearingsDate(model.NewHearing);

                        if (isPleadingHearingExists)
                            errors.Add("غير مسموح بإضافة جلسة نطق بالحكم بتاريخ سابق لجلسة مرافعة");
                    }
                }

                bool isReceivedJudgmentCase = await _hearingRepository.IsReceivedJudgmentCase(model.CaseId);
                var dbHearing = await _hearingRepository.GetAsync(model.Id);
                model.AssignedToId = dbHearing.AssignedToId;
                //if edited hearing changed to be pleading and recieving judgment date was recieved before ALL JUdGMENT DATA will be deleted 
                if (isReceivedJudgmentCase && dbHearing.IsPronouncedJudgment.Value && model.Status != HearingStatuses.Closed)
                {
                    if (model.Type == HearingTypes.Pleading)
                    {
                        await _hearingRepository.RemoveReceivingJudgmentDataAsync(model.Id);
                        await _caseService.RemoveJudgmentInstrumentAsync(model.CaseId);
                        var caseTransactionDto = new CaseTransactionDto()
                        {
                            CaseId = model.CaseId,
                            TransactionType = CaseTransactionTypes.RemoveReceivingJudgmentDate,
                            Note = "تم حذف بيانات الحكم"
                        };

                        await _caseService.AddTransactionAsync(caseTransactionDto);
                    }

                }
                else if (isReceivedJudgmentCase && (!dbHearing.IsPronouncedJudgment.Value && model.Status != HearingStatuses.Closed))
                {
                    errors.Add("غير مسموح بتعديل الجلسة لانه تم استلام موعد استلام الحكم لجلسة النطق بالحكم");
                }

                if (model.Type == HearingTypes.PronouncingJudgment)
                {
                    bool isPleadingHearingExists = await _hearingRepository.CheckPleadingHearingsDate(model);

                    if (isPleadingHearingExists)
                        errors.Add("غير مسموح بإضافة جلسة نطق بالحكم بتاريخ سابق لجلسة مرافعة");
                }

                var caseDetails = await _caseService.GetAsync(model.CaseId);

                if (caseDetails.Data.StartDate > model.HearingDate)
                    errors.Add("غير مسموح بإضافة جلسة بتاريخ سابق لتاريخ بداية القضية");

                if (errors.Any())
                {
                    return new ReturnResult<HearingDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = errors,
                        Data = null
                    };
                }
                await _hearingRepository.EditAsync(model);

                await _attachmentService.UpdateListAsync(model.Attachments);


                if (model.WithNewHearing == true)
                {
                    if (caseDetails.Data.Researchers.Count > 0)
                        model.NewHearing.AssignedToId = caseDetails.Data.Researchers.LastOrDefault().Id;
                    await _hearingRepository.AddAsync(model.NewHearing);
                }

                // Add transaction
                if (model.Summary != null)
                {

                    var caseTransactionDto = new CaseTransactionDto()
                    {
                        CaseId = model.CaseId,
                        Note = "تم غلق جلسة من نوع " + EnumExtensions.GetDescription(model.Type) + " بتاريخ: "
                        + DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) + " " + DateTimeHelper.GetHigriDate(DateTime.Now),
                        TransactionType = CaseTransactionTypes.AddHearingSummary
                    };
                    await _caseService.AddTransactionAsync(caseTransactionDto);
                }
                if (isReceivedJudgmentCase && model.IsPronouncedJudgment.Value && model.Status == HearingStatuses.Closed)
                {
                    await NotifyUsersWithObjctionLimitation(model);
                }
                return new ReturnResult<HearingDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<HearingDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string>() { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<bool>> RemoveAsync(int id)
        {
            try
            {

                await _hearingRepository.RemoveAsync(id);

                return new ReturnResult<bool>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
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
                    ErrorList = new List<string>() { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<HearingDto>> ReceivingJudgmentAsync(ReceivingJudgmentDto model)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new ReceivingJudgmentValidator(), model);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<HearingDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                var caseDetails = (await _caseService.GetAsync(model.CaseId)).Data;

                if (caseDetails == null)
                {
                    return new ReturnResult<HearingDto>(false, HttpStatuses.Status400BadRequest, new List<string> { "لا يمكن إتمام الإجراء لعدم وجود القضية." });
                }

                if (caseDetails.ReceivingJudgmentDate == model.ReceivingJudgmentDate && caseDetails.PronouncingJudgmentDate == model.PronouncingJudgmentDate)
                {
                    return new ReturnResult<HearingDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = new List<string> { "لا يوجد تعديل فى البيانات." },
                        Data = null
                    };
                }

                await _hearingRepository.ReceivingJudgmentAsync(model);
                var Hearing = await _hearingRepository.GetAsync(model.HearingId);
                var caseTransactionDto = new CaseTransactionDto()
                {
                    CaseId = model.CaseId,
                    TransactionType = caseDetails.ReceivingJudgmentDate == null ? CaseTransactionTypes.RecordReceivingJudgmentDate : CaseTransactionTypes.EditReceivingJudgmentDate,
                    Note = caseDetails.ReceivingJudgmentDate == null ?
                    $"تم استلام موعد الحكم فى تاريخ {model.ReceivingJudgmentDate.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) + "  " + DateTimeHelper.GetHigriDateForPrint(model.ReceivingJudgmentDate)}ومهلة الاعتراض على الحكم حتى" +
                    $" {Hearing.Case.ReceivingJudgmentDate.Value.AddDays(30).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) + "  " + DateTimeHelper.GetHigriDateForPrint(Hearing.Case.ReceivingJudgmentDate.Value.AddDays(30))}"
                   : $"تم تعديل موعد استلام الحكم الى تاريخ {model.ReceivingJudgmentDate.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) + "  " + DateTimeHelper.GetHigriDateForPrint(model.ReceivingJudgmentDate)} ومهلة الاعتراض على الحكم حتى" +
                    $"{Hearing.Case.ReceivingJudgmentDate.Value.AddDays(30).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) + "  " + DateTimeHelper.GetHigriDateForPrint(Hearing.Case.ReceivingJudgmentDate.Value.AddDays(30))}"
                };

                await _caseService.AddTransactionAsync(caseTransactionDto);

                return new ReturnResult<HearingDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<HearingDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<int>> GetMaxHearingNumberAsync(int caseId)
        {
            try
            {
                int maxHearingNumber = await _hearingRepository.GetMaxHearingNumberAsync(caseId);

                return new ReturnResult<int>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = maxHearingNumber
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, caseId);

                return new ReturnResult<int>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string>() { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<int>> GetFirstHearingIdAsync(int caseId)
        {
            try
            {
                int firstHearingId = await _hearingRepository.GetFirstHearingIdAsync(caseId);

                return new ReturnResult<int>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = firstHearingId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, caseId);

                return new ReturnResult<int>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string>() { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<bool>> IsHearingNumberExistsAsync(HearingNumberDto hearingDto)
        {
            try
            {
                bool isExists = await _hearingRepository.IsHearingNumberExistsAsync(hearingDto);

                return new ReturnResult<bool>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = isExists
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, hearingDto.HearingNumber);

                return new ReturnResult<bool>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string>() { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<QueryResultDto<HearingListItemDto>>> GetUpcomingHearingsAsync(int days)
        {
            try
            {
                var entities = await _hearingRepository.GetUpcomingHearingsAsync(days);

                return new ReturnResult<QueryResultDto<HearingListItemDto>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = entities
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, days);

                return new ReturnResult<QueryResultDto<HearingListItemDto>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status200OK,
                    ErrorList = new List<string>() { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }

        }

        public async Task<ReturnResult<string>> SendUpcomingHearingsNotificationsToConsultantAsync(int days)
        {
            try
            {
                var hearings = await _hearingRepository.GetUpcomingHearingsAsync(days);

                var email = new EmailNotification(_appSettings, null);

                string report = @"
                Report: Upcoming Notifications to Consultant
                ============================================
                Date: " + DateTime.Now.ToString("dd-MMM-yyyy") + @"
                Time: " + DateTime.Now.ToString("hh:mm");

                report += @"
                Hearings Count: " + hearings.TotalItems + Environment.NewLine;

                foreach (var hearing in hearings.Items)
                {
                    //var to = hearing.Case.Consultant.Email;
                    //var name = hearing.Case.Consultant.FirstName;
                    var hearingDate = hearing.HearingDate;
                    var subject = hearing.Case.Subject;

                    //string emailText = EmailHearingsNotifications(false, "نحيطكم علماً بانه لديك جلسة يوم", name, hearingDate, subject, "");


                    //if (!string.IsNullOrEmpty(to))
                    //{
                    //    if (email.Send(to, "تذكير بحضور جلسة", email.GetTemplate(emailText)))
                    //        report += to + "===> sent";
                    //    else
                    //        report += to + "===> fail !!";

                    //    report += Environment.NewLine;
                    //}
                }



                return new ReturnResult<string>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = report
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, days);

                return new ReturnResult<string>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status200OK,
                    ErrorList = new List<string>() { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<string>> SendUpcomingHearingsNotificationsToAdministratorAsync(int days)
        {
            try
            {
                var hearings = await _hearingRepository.GetUpcomingHearingsAsync(days);

                var email = new EmailNotification(_appSettings, null);

                string report = @"
                Report: Upcoming Notifications to Consultant
                ============================================
                Date: " + DateTime.Now.ToString("dd-MMM-yyyy") + @"
                Time: " + DateTime.Now.ToString("hh:mm");

                report += @"
                    Hearings Count: " + hearings.TotalItems + Environment.NewLine;

                var administrators = (await _userService.GetAllAsync(
                    new UserQueryObject()
                    {
                        Roles = "administratorOfJudiciary"
                    })).Data;

                foreach (var hearing in hearings.Items)
                {
                    //loop on all administrators of Judiciary
                    foreach (var admin in administrators.Items)
                    {
                        var to = admin.Email;
                        var name = admin.FirstName;
                        var hearingDate = hearing.HearingDate;
                        var subject = hearing.Case.Subject;

                        //string emailText = EmailHearingsNotifications(true, "نحيطكم علماً بانه يوجد جلسة يوم", name, hearingDate, subject, hearing.Case.Consultant.FirstName);


                        //if (!string.IsNullOrEmpty(to))
                        //{
                        //    if (email.Send(to, "التذكير بالجلسة", email.GetTemplate(emailText)))
                        //        report += to + "===> sent";
                        //    else
                        //        report += to + "===> fail !!";

                        //    report += Environment.NewLine;
                        //}
                    }
                }


                return new ReturnResult<string>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = report
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, days);

                return new ReturnResult<string>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status200OK,
                    ErrorList = new List<string>() { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }

        }

        public async Task<ReturnResult<string>> SendClosingHearingsNotificationsToConsultantAsync(int days)
        {
            try
            {
                var hearings = await _hearingRepository.GetUnclosedHearingsAsync(days);

                string report = @"
                Report: Reminder Closing Hearing Notifications to Consultant
                ============================================================
                Date: " + DateTime.Now.ToString("dd-MMM-yyyy") + @"
                Time: " + DateTime.Now.ToString("hh:mm");

                report += @"
                Hearings Count: " + hearings.TotalItems + Environment.NewLine;

                var email = new EmailNotification(_appSettings, null);

                foreach (var hearing in hearings.Items)
                {
                    //var to = hearing.Case.Consultant.Email;
                    //var name = hearing.Case.Consultant.FirstName;
                    var hearingDate = hearing.HearingDate;
                    var subject = hearing.Case.Subject;

                    //string emailText = EmailHearingsNotifications(false, "نذكركم بإغلاق جلسة يوم", name, hearingDate, subject, "");


                    //if (!string.IsNullOrEmpty(to))
                    //{
                    //    if (email.Send(to, "التذكير بإغلاق جلسة", email.GetTemplate(emailText)))
                    //        report += to + "===> sent";
                    //    else
                    //        report += to + "===> fail !!";

                    //    report += Environment.NewLine;
                    //}
                }

                return new ReturnResult<string>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = report
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, days);

                return new ReturnResult<string>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status200OK,
                    ErrorList = new List<string>() { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }

        }

        public async Task<ReturnResult<string>> SendClosingHearingsNotificationsToAdminstratorAsync(int days)
        {
            try
            {
                var hearings = await _hearingRepository.GetUnclosedHearingsAsync(days);

                var email = new EmailNotification(_appSettings, null);

                string report = @"
                 Report: Reminder Closing Hearing Notifications to Administrator
                 ===============================================================
                 Date: " + DateTime.Now.ToString("dd-MMM-yyyy") + @"
                 Time: " + DateTime.Now.ToString("hh:mm");


                report += @"
                 Hearings Count: " + hearings.TotalItems + Environment.NewLine;

                var administrators = (await _userService.GetAllAsync(
                    new UserQueryObject()
                    {
                        Roles = "administratorOfJudiciary"
                    })).Data;

                foreach (var hearing in hearings.Items)
                {
                    //loop on all administrators of Judiciary
                    foreach (var admin in administrators.Items)
                    {
                        var to = admin.Email;
                        var name = admin.FirstName;
                        var hearingDate = hearing.HearingDate;
                        var subject = hearing.Case.Subject;

                        //string emailText = EmailHearingsNotifications(true, "نذكركم بإغلاق جلسة يوم", name, hearingDate, subject, hearing.Case.Consultant.FirstName);


                        //if (!string.IsNullOrEmpty(to))
                        //{
                        //    if (email.Send(to, "التذكير بإغلاق جلسة", email.GetTemplate(emailText)))
                        //        report += to + "===> sent";
                        //    else
                        //        report += to + "===> fail !!";

                        //    report += Environment.NewLine;
                        //}
                    }
                }

                return new ReturnResult<string>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = report
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, days);

                return new ReturnResult<string>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status200OK,
                    ErrorList = new List<string>() { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<KeyValuePairsDto<Guid>>> AssignHearingToAsync(int hearingId, Guid attendantId)
        {
            try
            {
                var errors = new List<string>();

                if (hearingId <= 0)
                {
                    errors.Add("رقم الجلسة غير صحيح.");
                }

                if (attendantId == Guid.Empty)
                {
                    errors.Add("رقم المكلف بالحضور غير صحيح.");
                }

                if (errors.Any())
                {
                    return new ReturnResult<KeyValuePairsDto<Guid>>(false, HttpStatuses.Status400BadRequest, errors);
                }

                var hearingOld = await _hearingRepository.GetAsync(hearingId);

                var attendant = await _hearingRepository.AssignHearingToAsync(hearingId, attendantId);

                //// send a notification to the current researcher and new researcher when changing the Assign
                await NotifyUsersWithAssignHearingTo(hearingOld, attendantId);

                return new ReturnResult<KeyValuePairsDto<Guid>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = attendant
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, attendantId);

                return new ReturnResult<KeyValuePairsDto<Guid>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string>() { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        private string EmailHearingsNotifications(bool isAdministrator, string emailTitle, string name, DateTime hearingDate, string subject, string consultantName)
        {
            string html = "";

            html += @"
                    <table style='font-family:Traditional Arabic;width: 100%;'>
                        <tr>
                            <td colspan='3' style='font-family:Traditional Arabic;font-size: 18pt;font-weight: bold;padding: 20px;'>هلا، " + name + @"</td>
                        </tr>
                        <tr>
                            <td colspan='3' style='font-family:Traditional Arabic;padding: 5px 20px 20px 0;font-size:18pt;'>" + emailTitle + @" " + DateTimeHelper.GetArDay(hearingDate) + @"</td>
                        </tr>
                        <tr>
                            <td style='font-family:Traditional Arabic;padding: 20px 20px 7px 0;color:#9E9FA1;font-size:15pt;'>اسم القضية</td>
                            <td style='font-family:Traditional Arabic;text-align:center;padding: 20px 20px 7px 0;color:#9E9FA1;font-size:15pt;'>تاريخ الجلسة</td>
                            <td style='font-family:Traditional Arabic;text-align:center;padding: 20px 20px 7px 0;color:#9E9FA1;font-size:15pt;'>الساعة</td>
                        </tr>
                        <tr>
                            <td style='font-family:Traditional Arabic;padding: 0 20px 0 0; border-left: 1px solid #9E9FA1;font-size:16pt;'>" + subject + @"</td>
                            <td style='font-family:Traditional Arabic;text-align:center;padding: 0 20px 0 0; border-left: 1px solid #9E9FA1;font-size:16pt;'>" + hearingDate.ToString("dd-MMM-yyyy") + @"</td>
                            <td style='font-family:Traditional Arabic;text-align:center;padding: 0 20px 0 0;font-size:16pt;'>" + hearingDate.ToString("hh:mm") + @"</td>
                        </tr>";

            if (isAdministrator)
                html += @"  
                        <tr>
                            <td colspan='3' style='font-family:Traditional Arabic;padding: 30px 20px 7px 0;color:#9E9FA1;font-size:15pt;'>اسم المستشار</td>
                        </tr>
                        <tr>
                            <td style='font-family:Traditional Arabic;padding: 0 20px 20px 0;font-size:16pt;'>" + consultantName + @"</td>
                        </tr>";

            html += @"        
                        <tr>
                            <td colspan='3' style='font-family:Traditional Arabic;padding: 30px 20px 7px 0;color:#9E9FA1;font-size:15pt;text-align: center;'>نسأل الله لكم التوفيق</td>
                        </tr>

                        <tr>
                            <td colspan='3' style='font-family:Traditional Arabic;padding: 20px;color:#DA3F7B;font-size:18pt;font-weight: bold;text-align: center;'>تم ارسال هذه الرساله آليا من النظام</td>
                        </tr>
                    </table>";

            return html;
        }

        public async Task<ReturnResult<HearingUpdateDetailsDto>> GetHearingUpdateAsync(int id)
        {
            try
            {
                var entitiy = await _hearingUpdateService.GetAsync(id);

                return new ReturnResult<HearingUpdateDetailsDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entitiy.Data
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<HearingUpdateDetailsDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string>() { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<HearingUpdateDto>> AddHearingUpdateAsync(HearingUpdateDto model)
        {
            try
            {
                var errors = new List<string>();

                var validationResult = ValidationResult.CheckModelValidation(new HearingUpdateValidator(), model);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                await _hearingUpdateService.AddAsync(model);

                await _attachmentService.UpdateListAsync(model.Attachments);


                return new ReturnResult<HearingUpdateDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<HearingUpdateDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string>() { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<HearingUpdateDto>> EditHearingUpdateAsync(HearingUpdateDto model)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new HearingUpdateValidator(), model);
                if (!validationResult.IsValid)
                {
                    return new ReturnResult<HearingUpdateDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = validationResult.Errors,
                        Data = null
                    };
                }

                await _hearingUpdateService.EditAsync(model);

                return new ReturnResult<HearingUpdateDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<HearingUpdateDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string>() { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<Task> FinishHearing()
        {
            try
            {
                _logger.LogInformation($"{nameof(FinishHearing)} is starting...");

                await _hearingRepository.FinishHearing();

                _logger.LogInformation($"{nameof(FinishHearing)} ended successfully.");

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Task.FromException(ex);
            }
        }

        public async Task<Task> CloseHearing()
        {
            try
            {
                _logger.LogInformation($"{nameof(CloseHearing)} is starting...");

                await _hearingRepository.CloseHearing();

                _logger.LogInformation($"{nameof(CloseHearing)} ended successfully.");

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Task.FromException(ex);
            }
        }

        public async Task<Task> NotifyUsersWithApproachHearingDate()
        {
            try
            {
                _logger.LogInformation($"{nameof(NotifyUsersWithApproachHearingDate)} job is starting...");

                var _approachHearings = await _hearingRepository.ApproachHearingByResearcher();

                foreach (var hearing in _approachHearings)
                {
                    await _notificationService.AddAsync(new NotificationDto
                    {
                        Text = "موعد الجلسة رقم " + hearing.HearingNumber + " للقضية رقم " + hearing.CaseId + " بالدائرة " + hearing.CircleNumber + "- محكمة " + hearing.Court.Name + "   بتاريخ " + DateTimeHelper.GetHigriDate(hearing.HearingDate) + " الساعة " + hearing.HearingTime,
                        UserIds = new List<Guid>() { (Guid)hearing.AssignedToId },
                        SendEmailMessage = false,
                        URL = "/hearings/view/" + hearing.Id
                    });
                }

                _logger.LogInformation($"{nameof(NotifyUsersWithApproachHearingDate)} job ended successfully.");

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Task.FromException(ex);
            }
        }

        public async Task NotifyUsersWithNewHearingDate(HearingDto model)
        {
            try
            {
                var _hearing = await _hearingRepository.GetAsync(model.Id);
                var userIds = new List<Guid>();

                // add researcher
                if (_hearing.AssignedTo != null)
                {
                    userIds.Add(_hearing.AssignedTo.Id);

                    // add consultant
                    var consultantId = (await _researchsConsultantService.GetConsultantAsync(_hearing.AssignedTo.Id)).Data.ConsultantId;
                    if (consultantId.HasValue)
                        userIds.Add((Guid)consultantId);
                }

                // add litigationManagers
                var litigationManagers = (await _userService.GetUsersByRolesAsync(ApplicationRolesConstants.DepartmentManager.Code, model.BranchId, (int)Departments.Litigation)).Data;

                userIds.AddRange(litigationManagers.Select(m => m.Id).ToList());

                await _notificationService.AddAsync(new NotificationDto
                {
                    Text = "تم تحديد موعد للجلسة رقم " + model.HearingNumber + " في القضية رقم " + model.CaseId + " في الدائرة رقم " + model.CircleNumber + " – محكمة  " + _hearing.Court.Name,
                    UserIds = userIds,
                    SendEmailMessage = false,
                    URL = "/hearings/view/" + model.Id
                });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public async Task<Task> NotifyResearcherToCompleteHearingOutOfDate()
        {
            try
            {
                _logger.LogInformation($"{nameof(NotifyResearcherToCompleteHearingOutOfDate)} job is starting...");

                var _hearings = await _hearingRepository.GetHearingOutOfDate();

                if (_hearings != null && _hearings.Count > 0)
                {
                    foreach (var _hearing in _hearings)
                    {
                        await _notificationService.AddAsync(new NotificationDto
                        {
                            Text = String.Format("{0} {1}  لم يقم باغلاق الجلسة رقم {2} فى القضية رقم {3} الدائرة رقم {4} محكمة {5}",
                                                  _hearing.AssignedTo.FirstName, _hearing.AssignedTo.LastName, _hearing.Id, _hearing.CaseId, _hearing.CircleNumber, _hearing.CourtId),
                            UserIds = new List<Guid>() { _hearing.AssignedToId.Value },
                            SendEmailMessage = false,
                            Type = NotificationTypes.Info,
                            URL = "/hearings/view/" + _hearing.Id
                        });
                    }
                }

                _logger.LogInformation($"{nameof(NotifyResearcherToCompleteHearingOutOfDate)} job ended successfully.");

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Task.FromException(ex);
            }
        }

        public async Task<Task> NotifyManagerWithInCompletedHearingOutOfDate()
        {
            try
            {
                _logger.LogInformation($"{nameof(NotifyManagerWithInCompletedHearingOutOfDate)} job is starting...");

                var _finishedHearings = await _hearingRepository.GetHearingsOrderedByResearcher();

                foreach (var _finishedhearing in _finishedHearings)
                {
                    var _hearing = _finishedhearing.Hearings
                        .Where(h => h.HearingDate.Date == DateTime.Now.Date.AddDays(-2))
                        .FirstOrDefault();

                    if (_hearing != null)
                    {
                        var litigationManagers = await _userService.GetUsersByRolesAsync(ApplicationRolesConstants.DepartmentManager.Code, _hearing.AssignedTo.BranchId, (int)Departments.Litigation);

                        await _notificationService.AddAsync(new NotificationDto
                        {
                            Text = String.Format(" {1} {0}  لم يقم باغلاق جلستين هذا الشهر", _hearing.AssignedTo.FirstName, _hearing.AssignedTo.LastName),
                            UserIds = litigationManagers.Data.Select(m => m.Id).ToList(),
                            SendEmailMessage = false,
                            URL = "/hearings/view/" + _hearing.Id
                        });
                        break;
                    }
                }

                _logger.LogInformation($"{nameof(NotifyManagerWithInCompletedHearingOutOfDate)} job ended successfully.");

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Task.FromException(ex);
            }
        }

        public async Task NotifyUsersWithObjctionLimitation(HearingDto model)
        {
            try
            {
                var caseDetails = (await _caseService.GetAsync(model.CaseId)).Data;
                if (caseDetails != null)
                {
                    var userIds = new List<Guid>();

                    // add researcher
                    var _caseResearchers = (await _caseResearcherService.GetByCaseAsync(caseDetails.Id)).Data;
                    if (_caseResearchers != null)
                        userIds.Add(_caseResearchers.ResearcherId);
                    // add consultant
                    var _researchsConsultant = (await _researchsConsultantService.GetConsultantAsync(_caseResearchers.ResearcherId)).Data;
                    if (_researchsConsultant != null)
                        userIds.Add(_researchsConsultant.ConsultantId.Value);
                    // add litigation manager
                    var litigationManagers = (await _userService.GetUsersByRolesAsync(ApplicationRolesConstants.DepartmentManager.Code, caseDetails.BranchId, (int)Departments.Litigation)).Data;
                    userIds.AddRange(litigationManagers.Select(m => m.Id).ToList());

                    await _notificationService.AddAsync(new NotificationDto
                    {
                        Text = String.Format("    تم بدء مهلة الاعتراض على القضية رقم {0} فى الدائرة رقم {1} محكمة {2} و تنتهى المهلة بتاريخ {3}  ", caseDetails.CaseNumberInSource, caseDetails.CircleNumber, caseDetails.Court.Name, Common.DateTimeHelper.GetHigriDate(caseDetails.ReceivingJudgmentDate.Value.AddDays(30))),
                        UserIds = userIds,
                        SendEmailMessage = true,
                        Type = NotificationTypes.Info,
                        URL = "cases/view/" + caseDetails.Id,
                        SendSMSMessage = true
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public async Task NotifyUsersWithAssignHearingTo(HearingDetailsDto hearingDetails, Guid newAttendantId)
        {
            var userIds = new List<Guid>();

            // add current researcher to list
            userIds.Add(hearingDetails.AssignedTo.Id);

            // add new researcher to list
            userIds.Add(newAttendantId);

            // send a notification to users when accept request
            var notificationDto = new NotificationDto()
            {
                Id = 0,
                Text = "تم تغيير المكلف بحضور الجلسة رقم الجلسة " + hearingDetails.HearingNumber,
                URL = "/hearings/view/" + hearingDetails.Id,
                UserIds = userIds
            };

            await _notificationService.AddAsync(notificationDto);
        }

        public async Task<ReturnResult<QueryResultDto<UserListItemDto>>> GetConsultantsAndResearchers(UserQueryObject queryObject)
        {
            try
            {
                var entities = await _hearingRepository.GetConsultantsAndResearchers(queryObject);

                return new ReturnResult<QueryResultDto<UserListItemDto>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entities
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, queryObject);

                return new ReturnResult<QueryResultDto<UserListItemDto>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }
    }
}