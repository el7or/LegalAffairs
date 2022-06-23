using Microsoft.Extensions.Logging;
using Moe.La.Core.Constants;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using Moe.La.ServiceInterface.Validators;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class MoamalaService : IMoamalaService
    {
        private readonly IMoamalaRepository _moamalaRepository;
        private readonly IUserService _userService;
        private readonly IMoamalaTransactionRepository _moamalaTransactionRepository;
        private readonly IAttachmentService _attachmentService;
        private readonly ILogger<MoamalaService> _logger;

        public MoamalaService(IMoamalaRepository moamalaRepository, IMoamalaTransactionRepository moamalaTransactionRepository, IAttachmentService attachmentService, ILogger<MoamalaService> logger, IUserService userService)
        {
            _moamalaRepository = moamalaRepository;
            _moamalaTransactionRepository = moamalaTransactionRepository;
            _attachmentService = attachmentService;
            _logger = logger;
            _userService = userService;
        }

        public async Task<ReturnResult<QueryResultDto<MoamalaListItemDto>>> GetAllAsync(MoamalatQueryObject queryObject)
        {
            try
            {
                var entities = await _moamalaRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<MoamalaListItemDto>>(true, HttpStatuses.Status200OK, entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, queryObject);

                return new ReturnResult<QueryResultDto<MoamalaListItemDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<MoamalaDetailsDto>> GetAsync(int id)
        {
            try
            {
                if (id < 1)
                {
                    return new ReturnResult<MoamalaDetailsDto>(false, HttpStatuses.Status400BadRequest, new List<string> { "رقم المعاملة المدخل غير صحيح" });
                }

                var entitiy = await _moamalaRepository.GetAsync(id);

                return new ReturnResult<MoamalaDetailsDto>(true, HttpStatuses.Status200OK, entitiy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<MoamalaDetailsDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<MoamalaDto>> AddAsync(MoamalaDto moamalaDto)
        {
            try
            {
                var errors = new List<string>();

                var validationResult = ValidationResult.CheckModelValidation(new MoamlaValidator(), moamalaDto);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }
                await _moamalaRepository.AddAsync(moamalaDto);

                if (moamalaDto.Attachments != null && moamalaDto.Attachments.Count > 0)
                {
                    await _attachmentService.UpdateListAsync(moamalaDto.Attachments);
                }

                return new ReturnResult<MoamalaDto>(true, HttpStatuses.Status200OK, moamalaDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, moamalaDto);

                return new ReturnResult<MoamalaDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<MoamalaDto>> EditAsync(MoamalaDto moamalaDto)
        {
            try
            {
                var errors = new List<string>();

                var validationResult = ValidationResult.CheckModelValidation(new MoamlaValidator(), moamalaDto);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }
                await _moamalaRepository.EditAsync(moamalaDto);

                await _attachmentService.UpdateListAsync(moamalaDto.Attachments);


                return new ReturnResult<MoamalaDto>(true, HttpStatuses.Status200OK, moamalaDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, moamalaDto);

                return new ReturnResult<MoamalaDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> RemoveAsync(int id)
        {
            try
            {

                await _moamalaRepository.RemoveAsync(id);

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
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<bool>> ChangeStatusAsync(MoamalaChangeStatusDto changeStatusDto)
        {
            try
            {
                var errors = new List<string>();

                var validationResult = ValidationResult.CheckModelValidation(new MoamalaChangeStatusValidator(), changeStatusDto);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }
                await _moamalaRepository.ChangeStatusAsync(changeStatusDto);

                // Add transaction  

                var moamalaTransactionDto = new MoamalaTransactionDto()
                {
                    MoamalaId = changeStatusDto.MoamalaId,
                    Note = changeStatusDto.Note
                };

                if (changeStatusDto.Status == MoamalaStatuses.Referred && changeStatusDto.DepartmentId == null)
                {
                    moamalaTransactionDto.TransactionType = MoamalaTransactionTypes.ReferredTransaction;

                    // add notifications to users
                    var generalMangers = await _userService.GetUsersByRolesAsync(ApplicationRolesConstants.BranchManager.Code, changeStatusDto.BranchId);

                    foreach (var generalManger in generalMangers.Data)
                    {
                        await _moamalaRepository.AddNotificationAsync(new MoamalaNotificationDto
                        {
                            UserId = generalManger.Id,
                            MoamalaId = changeStatusDto.MoamalaId,
                            IsRead = false
                        });
                    }
                }

                else if (changeStatusDto.Status == MoamalaStatuses.Referred && changeStatusDto.DepartmentId != null)
                {
                    moamalaTransactionDto.TransactionType = MoamalaTransactionTypes.ReferredTransaction;
                    // add notifications to users
                    var departmentManager = await _userService.GetDepartmentManagerAsync((int)changeStatusDto.DepartmentId);

                    await _moamalaRepository.AddNotificationAsync(new MoamalaNotificationDto
                    {
                        UserId = departmentManager.Data.Id,
                        MoamalaId = changeStatusDto.MoamalaId,
                        IsRead = false
                    });
                }

                else if (changeStatusDto.Status == MoamalaStatuses.Assigned)
                {
                    moamalaTransactionDto.TransactionType = MoamalaTransactionTypes.AssignTransaction;
                    // add notification to assignedto user
                    await _moamalaRepository.AddNotificationAsync(new MoamalaNotificationDto
                    {
                        UserId = (Guid)changeStatusDto.AssignedToId,
                        MoamalaId = changeStatusDto.MoamalaId,
                        IsRead = false
                    });

                }

                else if (changeStatusDto.Status == MoamalaStatuses.MoamalaReturned)
                {
                    moamalaTransactionDto.TransactionType = MoamalaTransactionTypes.ReturnTransaction;
                    var entityToUpdate = _moamalaRepository.GetAsync(changeStatusDto.MoamalaId).Result;

                    if (entityToUpdate.CurrentStep == MoamalaSteps.Initial)
                    {
                        var generalSupervisors = _userService.GetAllAsync(new UserQueryObject
                        {
                            Roles = ApplicationRolesConstants.GeneralSupervisor.Name
                        });
                        foreach (var generalSupervisor in generalSupervisors.Result.Data.Items)
                        {
                            await _moamalaRepository.AddNotificationAsync(new MoamalaNotificationDto
                            {
                                UserId = generalSupervisor.Id,
                                MoamalaId = changeStatusDto.MoamalaId,
                                IsRead = false
                            });

                        }
                        var distributersHasConfidential = _userService.GetAllAsync(new UserQueryObject
                        {
                            Roles = ApplicationRolesConstants.Distributor.Name,
                            HasConfidentialPermission = (entityToUpdate.ConfidentialDegree.Id == (int)ConfidentialDegrees.Confidential)
                        });
                        foreach (var distributer in distributersHasConfidential.Result.Data.Items)
                        {
                            await _moamalaRepository.AddNotificationAsync(new MoamalaNotificationDto
                            {
                                UserId = distributer.Id,
                                MoamalaId = changeStatusDto.MoamalaId,
                                IsRead = false
                            });
                        }
                    }
                    else if (entityToUpdate.CurrentStep == MoamalaSteps.Branch)
                    {

                        var generalMangers = await _userService.GetUsersByRolesAsync(ApplicationRolesConstants.BranchManager.Code, changeStatusDto.BranchId);

                        foreach (var generalManger in generalMangers.Data)
                        {
                            await _moamalaRepository.AddNotificationAsync(new MoamalaNotificationDto
                            {
                                UserId = generalManger.Id,
                                MoamalaId = changeStatusDto.MoamalaId,
                                IsRead = false
                            });
                        }

                    }
                    else if (entityToUpdate.CurrentStep == MoamalaSteps.Department)
                    {
                        var departmentManager = _userService.GetDepartmentManagerAsync((int)changeStatusDto.DepartmentId);
                        await _moamalaRepository.AddNotificationAsync(new MoamalaNotificationDto
                        {
                            UserId = departmentManager.Result.Data.Id,
                            MoamalaId = changeStatusDto.MoamalaId,
                            IsRead = false
                        });
                    }
                }

                await AddTransactionAsync(moamalaTransactionDto);


                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, false);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> ReturnAsync(int id, string note)
        {
            try
            {

                await _moamalaRepository.ReturnAsync(id, note);

                // Add transaction  

                var moamalaTransactionDto = new MoamalaTransactionDto()
                {
                    MoamalaId = id,
                    Note = note
                };


                moamalaTransactionDto.TransactionType = MoamalaTransactionTypes.ReturnTransaction;
                var entityToUpdate = _moamalaRepository.GetAsync(id).Result;

                if (entityToUpdate.CurrentStep == MoamalaSteps.Initial)
                {
                    var generalSupervisors = _userService.GetAllAsync(new UserQueryObject
                    {
                        Roles = ApplicationRolesConstants.GeneralSupervisor.Name
                    });
                    foreach (var generalSupervisor in generalSupervisors.Result.Data.Items)
                    {
                        await _moamalaRepository.AddNotificationAsync(new MoamalaNotificationDto
                        {
                            UserId = generalSupervisor.Id,
                            MoamalaId = id,
                            IsRead = false
                        });

                    }
                    var distributersHasConfidential = _userService.GetAllAsync(new UserQueryObject
                    {
                        Roles = ApplicationRolesConstants.Distributor.Name,
                        HasConfidentialPermission = (entityToUpdate.ConfidentialDegree.Id == (int)ConfidentialDegrees.Confidential)
                    });
                    foreach (var distributer in distributersHasConfidential.Result.Data.Items)
                    {
                        await _moamalaRepository.AddNotificationAsync(new MoamalaNotificationDto
                        {
                            UserId = distributer.Id,
                            MoamalaId = id,
                            IsRead = false
                        });
                    }
                }
                else if (entityToUpdate.CurrentStep == MoamalaSteps.Branch)
                {
                    var generalMangers = await _userService.GetUsersByRolesAsync(ApplicationRolesConstants.BranchManager.Code, entityToUpdate.Branch.Id);

                    foreach (var generalManger in generalMangers.Data)
                    {
                        await _moamalaRepository.AddNotificationAsync(new MoamalaNotificationDto
                        {
                            UserId = generalManger.Id,
                            MoamalaId = id,
                            IsRead = false
                        });
                    }

                }
                else if (entityToUpdate.CurrentStep == MoamalaSteps.Department)
                {
                    var departmentManager = _userService.GetDepartmentManagerAsync((int)entityToUpdate.ReceiverDepartment.Id);
                    await _moamalaRepository.AddNotificationAsync(new MoamalaNotificationDto
                    {
                        UserId = departmentManager.Result.Data.Id,
                        MoamalaId = id,
                        IsRead = false
                    });
                }


                await AddTransactionAsync(moamalaTransactionDto);


                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, false);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<MoamalaDetailsDto>> UpdateWorkItemTypeAsync(MoamalaUpdateWorkItemType model)
        {
            try
            {
                var result = await _moamalaRepository.UpdateWorkItemTypeAsync(model);

                return new ReturnResult<MoamalaDetailsDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<MoamalaDetailsDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<MoamalaDetailsDto>> UpdateRelatedIdAsync(MoamalaUpdateRelatedId model)
        {
            try
            {
                var result = await _moamalaRepository.UpdateRelatedIdAsync(model);

                return new ReturnResult<MoamalaDetailsDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status201Created,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<MoamalaDetailsDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }
        public async Task<ReturnResult<MoamalaTransactionDto>> AddTransactionAsync(MoamalaTransactionDto moamalaTransactionDto)
        {
            try
            {
                var errors = new List<string>();

                var validationResult = ValidationResult.CheckModelValidation(new MoamalaTransactionValidator(), moamalaTransactionDto);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }
                await _moamalaTransactionRepository.AddAsync(moamalaTransactionDto);

                return new ReturnResult<MoamalaTransactionDto>(true, HttpStatuses.Status200OK, moamalaTransactionDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, moamalaTransactionDto);

                return new ReturnResult<MoamalaTransactionDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

    }
}
