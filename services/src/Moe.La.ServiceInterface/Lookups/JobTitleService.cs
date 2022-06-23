using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using Moe.La.ServiceInterface.Validators.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class JobTitleService : IJobTitleService
    {
        private readonly IJobTitleRepository _jobTitleRepository;
        private readonly IUserService _userService;
        private readonly ILogger<JobTitleService> _logger;

        public JobTitleService(IJobTitleRepository jobTitleRepository, IUserService userService, ILogger<JobTitleService> logger)
        {
            _jobTitleRepository = jobTitleRepository;
            _userService = userService;
            _logger = logger;
        }

        public async Task<ReturnResult<QueryResultDto<JobTitleListItemDto>>> GetAllAsync(QueryObject queryObject)
        {
            try
            {
                var entities = await _jobTitleRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<JobTitleListItemDto>>(true, HttpStatuses.Status200OK, entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, queryObject);

                return new ReturnResult<QueryResultDto<JobTitleListItemDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<JobTitleListItemDto>> GetAsync(int id)
        {
            try
            {
                var entity = await _jobTitleRepository.GetAsync(id);

                return new ReturnResult<JobTitleListItemDto>(true, HttpStatuses.Status200OK, entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<JobTitleListItemDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<JobTitleDto>> AddAsync(JobTitleDto model)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new JobTitleValidator(), model);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                bool isJobTitleExists = await _jobTitleRepository.IsNameExistsAsync(model.Name);
                if (isJobTitleExists)
                {
                    errors.Add("المسمى الوظيفى موجود مسبقاً.");
                }

                if (errors.Any())
                {
                    return new ReturnResult<JobTitleDto>(false, HttpStatuses.Status400BadRequest, errors);
                }

                await _jobTitleRepository.AddAsync(model);

                return new ReturnResult<JobTitleDto>(true, HttpStatuses.Status201Created, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<JobTitleDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<JobTitleDto>> EditAsync(JobTitleDto model)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new JobTitleValidator(), model);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }
                var dbJobTitle = await _jobTitleRepository.GetAsync(model.Id);
                bool isJobTitleExists = await _jobTitleRepository.IsNameExistsAsync(model.Name);
                if (isJobTitleExists && model.Name != dbJobTitle.Name)
                {
                    errors.Add("المسمى الوظيفى موجود مسبقاً.");
                }

                if (errors.Any())
                {
                    return new ReturnResult<JobTitleDto>(false, HttpStatuses.Status400BadRequest, errors);
                }

                await _jobTitleRepository.EditAsync(model);

                return new ReturnResult<JobTitleDto>(true, HttpStatuses.Status200OK, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<JobTitleDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> RemoveAsync(int id)
        {
            try
            {
                //var errors = new List<string>();

                //var CheckJobTitleExists = await _userService.CheckJobTitleExists(id);
                //if (CheckJobTitleExists.Data)
                //{
                //    errors.Add("المسمى الوظيفى مضاف لمستخدمين");

                //    return new ReturnResult<bool>
                //    {
                //        IsSuccess = false,
                //        StatusCode = HttpStatuses.Status400BadRequest,
                //        ErrorList = errors
                //    };
                //}

                await _jobTitleRepository.RemoveAsync(id);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, true);
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

        public async Task<ReturnResult<bool>> IsNameExistsAsync(string name)
        {
            try
            {
                var errors = new List<string>();

                bool result = await _jobTitleRepository.IsNameExistsAsync(name);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, name);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }
    }
}
