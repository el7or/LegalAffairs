using Microsoft.Extensions.Logging;
using Moe.La.Core.Constants;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using Moe.La.ServiceInterface.Validators.Others;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class LegalBoardService : ILegalBoardService
    {
        private readonly ILegalBoardRepository _legalBoardRepository;
        private readonly IBoardMeetingRepository _boardMeetingRepository;
        private readonly ILogger<LegalBoardService> _logger;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        public LegalBoardService(ILegalBoardRepository legalBoardRepository, ILogger<LegalBoardService> logger,
            IUserService userService, IBoardMeetingRepository boardMeetingRepository, INotificationService notificationService)
        {
            _legalBoardRepository = legalBoardRepository;
            _boardMeetingRepository = boardMeetingRepository;
            _logger = logger;
            _userService = userService;
            _notificationService = notificationService;
        }

        public async Task<ReturnResult<QueryResultDto<LegalBoardListItemDto>>> GetAllAsync(LegalBoardQueryObject queryObject)
        {
            try
            {
                var entities = await _legalBoardRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<LegalBoardListItemDto>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entities
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<QueryResultDto<LegalBoardListItemDto>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<LegalBoardDetailsDto>> GetAsync(int id)
        {
            try
            {
                var entitiy = await _legalBoardRepository.GetAsync(id);

                return new ReturnResult<LegalBoardDetailsDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entitiy
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<LegalBoardDetailsDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<LegalBoardDto>> AddAsync(LegalBoardDto model)
        {
            try
            {
                var errors = new List<string>();

                var validationResult = ValidationResult.CheckModelValidation(new LegalBoardValidator(), model);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                //  check if there  is  Major legalBoard
                if (model.TypeId == LegalBoardTypes.Major)
                {
                    var isExist = await _legalBoardRepository.CheckMajorLegalBoardAsync(null);
                    if (isExist == true)
                        errors.Add("يوجد لجنة رئيسية بالفعل لا يمكنك إضافة لجنة رئيسية أخرى.");
                }
                if (errors.Any())
                {
                    return new ReturnResult<LegalBoardDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = errors
                    };
                }
                await _legalBoardRepository.AddAsync(model);
                foreach (var item in model.LegalBoardMembers)
                {
                    if (item.IsActive)
                    {
                        if (item.MembershipType == LegalBoardMembershipTypes.Head && model.TypeId == LegalBoardTypes.Major)
                        {
                            await _userService.AssignRoleToUserAsync(item.UserId, ApplicationRolesConstants.MainBoardHead.Name);
                        }
                        else if (item.MembershipType == LegalBoardMembershipTypes.Member)
                        {
                            await _userService.AssignRoleToUserAsync(item.UserId, ApplicationRolesConstants.BoardMember.Name);
                        }
                        else if (item.MembershipType == LegalBoardMembershipTypes.Head)
                            await _userService.AssignRoleToUserAsync(item.UserId, ApplicationRolesConstants.SubBoardHead.Name);
                    }
                }

                // Add   members history to record join periods.
                await _legalBoardRepository.AddLegalBoardMemberHistoryAsync(model.LegalBoardMembers.ToList(), model.Id);

                return new ReturnResult<LegalBoardDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<LegalBoardDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<LegalBoardDto>> EditAsync(LegalBoardDto model)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new LegalBoardValidator(), model);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }
                //  check if there  is  Major legalBoard
                if (model.TypeId == LegalBoardTypes.Major)
                {
                    var isExist = await _legalBoardRepository.CheckMajorLegalBoardAsync(model.Id);
                    if (isExist == true)
                        errors.Add("يوجد لجنة رئيسية بالفعل لا يمكنك إضافة لجنة رئيسية أخرى.");
                }

                if (model.LegalBoardMembers.Where(m => m.MembershipType == LegalBoardMembershipTypes.Head).Count() > 1)
                {
                    errors.Add("لا يمكن إضافة اكثر من امين لجنة فى نفس اللجنة");
                }

                if (model.LegalBoardMembers.Where(m => m.MembershipType == LegalBoardMembershipTypes.Head).Count() == 0)
                {
                    errors.Add("اللجنة يجب ان تحتوى على أمين لجنة على الأقل");
                }
                if (errors.Any())
                {
                    return new ReturnResult<LegalBoardDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = errors
                    };
                }
                await _legalBoardRepository.EditAsync(model);
                foreach (var item in model.LegalBoardMembers)
                {
                    if (item.IsActive)
                    {
                        if (item.MembershipType == LegalBoardMembershipTypes.Head && model.TypeId == LegalBoardTypes.Major)
                        {
                            await _userService.AssignRoleToUserAsync(item.UserId, ApplicationRolesConstants.MainBoardHead.Name);
                        }
                        else if (item.MembershipType == LegalBoardMembershipTypes.Member)
                        {
                            await _userService.AssignRoleToUserAsync(item.UserId, ApplicationRolesConstants.BoardMember.Name);
                        }
                        else if (item.MembershipType == LegalBoardMembershipTypes.Head)
                            await _userService.AssignRoleToUserAsync(item.UserId, ApplicationRolesConstants.SubBoardHead.Name);
                    }
                }




                await _legalBoardRepository.AddLegalBoardMemberHistoryAsync(model.LegalBoardMembers.ToList(), model.Id);

                return new ReturnResult<LegalBoardDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<LegalBoardDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<bool>> ChangeStatusAsync(int id, int isActive)
        {
            try
            {
                await _legalBoardRepository.ChangeStatusAsync(id, isActive);
                return new ReturnResult<bool>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<bool>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<bool>> RemoveAsync(int id)
        {
            try
            {
                await _legalBoardRepository.RemoveAsync(id);
                return new ReturnResult<bool>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<bool>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<ICollection<LegalBoardSimpleDto>>> GetLegalBoard()
        {
            try
            {
                var entities = await _legalBoardRepository.GetLegalBoard();

                return new ReturnResult<ICollection<LegalBoardSimpleDto>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entities
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<ICollection<LegalBoardSimpleDto>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        #region legal-board-memo

        public async Task<ReturnResult<LegalBoardMemoDto>> AddLegalBoardMemoAsync(LegalBoardMemoDto model)
        {
            try
            {
                var errors = new List<string>();

                var validationResult = ValidationResult.CheckModelValidation(new LegalBoardMemoValidator(), model);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                // get LegalBoardId of major board
                if (model.LegalBoardId == 0)
                {
                    model.LegalBoardId = await _legalBoardRepository.GetMajorLegalBoardId();

                    if (model.LegalBoardId == 0)
                        errors.Add("لا توجد لجنة رئيسية لاحالة المذكرة اليها");
                }

                if (errors.Any())
                {
                    return new ReturnResult<LegalBoardMemoDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = errors
                    };
                }


                await _legalBoardRepository.AddLegalBoardMemoAsync(model);
                return new ReturnResult<LegalBoardMemoDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = model
                };
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<LegalBoardMemoDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<LegalBoardMemoDto>> EditLegalBoardMemoAsync(LegalBoardMemoDto model)
        {
            try
            {
                var errors = new List<string>();

                var validationResult = ValidationResult.CheckModelValidation(new LegalBoardMemoValidator(), model);

                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                if (errors.Any())
                {
                    return new ReturnResult<LegalBoardMemoDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = errors
                    };
                }
                // get LegalBoardId of major board
                if (model.LegalBoardId == 0)
                {
                    model.LegalBoardId = await _legalBoardRepository.GetMajorLegalBoardId();
                }

                await _legalBoardRepository.EditLegalBoardMemoAsync(model);

                return new ReturnResult<LegalBoardMemoDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = model
                };
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<LegalBoardMemoDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        #endregion

        #region legal-board-members
        public async Task<ReturnResult<QueryResultDto<LegalBoardMemberHistoryDto>>> GetLegalBoardMemberHistory(LegalBoardMemberQueryObject queryObject)
        {
            try
            {
                var entitiy = await _legalBoardRepository.GetLegalBoardMemberHistory(queryObject);

                return new ReturnResult<QueryResultDto<LegalBoardMemberHistoryDto>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entitiy
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<QueryResultDto<LegalBoardMemberHistoryDto>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<List<LegalBoradMemberDto>>> GetLegalBoardMembers(int boardId)
        {
            try
            {
                var legalBoardmembers = await _legalBoardRepository.GetLegalBoardMembers(boardId);

                return new ReturnResult<List<LegalBoradMemberDto>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = legalBoardmembers
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<List<LegalBoradMemberDto>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        #endregion


        #region board-meeting
        public async Task<ReturnResult<QueryResultDto<BoardMeetingListItemDto>>> GetAllBoardMeetingAsync(BoardMeetingQueryObject queryObject)
        {
            try
            {
                var entities = await _boardMeetingRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<BoardMeetingListItemDto>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entities
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<QueryResultDto<BoardMeetingListItemDto>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<BoardMeetingDetailsDto>> GetBoardMeetingAsync(int id)
        {
            try
            {
                if (id < 1)
                {
                    return new ReturnResult<BoardMeetingDetailsDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = new List<string> { "الرقم المدخل غير صحيح" }
                    };
                }
                var isCurrentUserMemberInMeeting = await _boardMeetingRepository.CheckIsCurrentUserMemberInMeeting(id);
                if (!isCurrentUserMemberInMeeting)
                {
                    return new ReturnResult<BoardMeetingDetailsDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = new List<string> { "لست ضمن المكلفين بمراجعة المذكرة" }
                    };
                }
                var entity = await _boardMeetingRepository.GetAsync(id);

                return new ReturnResult<BoardMeetingDetailsDto>
                {
                    Data = entity,
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);
                return new ReturnResult<BoardMeetingDetailsDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status404NotFound,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<BoardMeetingDetailsDto>> GetMeetingByBoardAndMemoAsync(int boardId, int memoId)
        {
            try
            {
                if (boardId < 1 || memoId < 1)
                {
                    return new ReturnResult<BoardMeetingDetailsDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = new List<string> { "الرقم المدخل غير صحيح" }
                    };
                }

                var entity = await _boardMeetingRepository.GetByBoardAndMemoAsync(boardId, memoId);


                return new ReturnResult<BoardMeetingDetailsDto>
                {
                    Data = entity,
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);
                return new ReturnResult<BoardMeetingDetailsDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status404NotFound,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<BoardMeetingDto>> AddBoardMeetingAsync(BoardMeetingDto model)
        {
            try
            {
                var errors = new List<string>();

                var validationResult = ValidationResult.CheckModelValidation(new BoardMeetingValidator(), model);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                if (errors.Any())
                {
                    return new ReturnResult<BoardMeetingDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = errors
                    };
                }

                await _boardMeetingRepository.AddAsync(model);

                var meeting = await _boardMeetingRepository.GetByBoardAndMemoAsync(model.BoardId, model.MemoId);

                if (model.BoardMeetingMembers.ToList().Count() > 0)
                    await NotifyBoardMemberForNewMemo(meeting, model.BoardMeetingMembers.ToList());


                return new ReturnResult<BoardMeetingDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = model
                };
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<BoardMeetingDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<BoardMeetingDto>> EditBoardMeetingAsync(BoardMeetingDto model)
        {
            try
            {
                var errors = new List<string>();

                var validationResult = ValidationResult.CheckModelValidation(new BoardMeetingValidator(), model);

                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                if (errors.Any())
                {
                    return new ReturnResult<BoardMeetingDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = errors
                    };
                }

                // get new added members to meeting 
                var oldMeetingMembers = await _boardMeetingRepository.GetCurrentMeetingMembers((int)model.Id);
                var membersToNotify = model.BoardMeetingMembers.Where(m => !oldMeetingMembers.Contains(m)).ToList();

                await _boardMeetingRepository.EditAsync(model);

                if (membersToNotify.Count > 0)
                {
                    var meeting = await _boardMeetingRepository.GetByBoardAndMemoAsync(model.BoardId, model.MemoId);

                    await NotifyBoardMemberForNewMemo(meeting, membersToNotify);
                }

                return new ReturnResult<BoardMeetingDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = model
                };
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return new ReturnResult<BoardMeetingDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<Task> NotifyBoardMemberForNewMemo(BoardMeetingDetailsDto meeting, List<int> boardMeetingMembers)
        {
            try
            {
                var userIds = new List<Guid>();

                foreach (var member in boardMeetingMembers)
                {

                    var memberUserId = await _legalBoardRepository.GetMemberUserIdAsync(member);
                    if (memberUserId.HasValue)
                        userIds.Add((Guid)memberUserId);
                }

                await _notificationService.AddAsync(new NotificationDto
                {
                    Text = "تم ورود المذكرة " + meeting.Memo.Name + " لكم لابداء الرأى فيها فى اللجنة ال" + meeting.LegalBoardType + " " + meeting.Board,
                    UserIds = userIds,
                    SendEmailMessage = true,
                    URL = "/memos/meeting-view/" + meeting.Id
                });


                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Task.FromException(ex);
            }
        }
        #endregion
    }
}
