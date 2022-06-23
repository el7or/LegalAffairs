using Microsoft.Extensions.Logging;//
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class InvestigationRecordService : IInvestigationRecordService
    {
        private readonly IInvestigationRecordRepository _investigationRecordRepository;
        private readonly ILogger<InvestigationRecordService> _logger;
        private readonly IInvestigationQuestionService _investigationQuestionService;
        private readonly IAttachmentService _attachmentService;

        public InvestigationRecordService(
            IInvestigationRecordRepository InvestigationRecordRepository,
            IInvestigationQuestionService InvestigationQuestionService,
            IAttachmentService attachmentService,
            ILogger<InvestigationRecordService> logger)
        {
            _investigationRecordRepository = InvestigationRecordRepository;
            _investigationQuestionService = InvestigationQuestionService;
            _attachmentService = attachmentService;
            _logger = logger;
        }

        public async Task<ReturnResult<QueryResultDto<InvestigationRecordListItemDto>>> GetAllAsync(InvestiationRecordQueryObject queryObject)
        {
            try
            {
                var entities = await _investigationRecordRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<InvestigationRecordListItemDto>>(true, HttpStatuses.Status200OK, entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, queryObject);

                return new ReturnResult<QueryResultDto<InvestigationRecordListItemDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<InvestigationRecordDetailsDto>> GetAsync(int id)
        {
            try
            {
                var entitiy = await _investigationRecordRepository.GetAsync(id);

                return new ReturnResult<InvestigationRecordDetailsDto>(true, HttpStatuses.Status200OK, entitiy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<InvestigationRecordDetailsDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<InvestigationRecordDto>> AddAsync(InvestigationRecordDto model)
        {
            try
            {

                if (model.InvestigationRecordQuestions != null)
                {
                    foreach (var question in model.InvestigationRecordQuestions)
                    {
                        var isQuestionExist = await _investigationQuestionService.IsNameExistAsync(question.Question);
                        if (!isQuestionExist.Data && question.QuestionId == null)
                        {
                            var addedQuestion = await _investigationQuestionService.AddAsync(new InvestigationQuestionDto { Question = question.Question, Status = InvestigationQuestionStatuses.Undefined });
                            question.QuestionId = addedQuestion.Data.Id;
                        }
                    }
                }

                await _investigationRecordRepository.AddAsync(model);

                await _attachmentService.UpdateListAsync(model.Attachments);


                return new ReturnResult<InvestigationRecordDto>(true, HttpStatuses.Status201Created, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<InvestigationRecordDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<InvestigationRecordDto>> EditAsync(InvestigationRecordDto model)
        {
            try
            {

                if (model.InvestigationRecordQuestions != null)
                {
                    foreach (var question in model.InvestigationRecordQuestions)
                    {
                        var isQuestionExist = await _investigationQuestionService.IsNameExistAsync(question.Question);
                        if (!isQuestionExist.Data && question.QuestionId == null)
                        {
                            var addedQuestion = await _investigationQuestionService.AddAsync(new InvestigationQuestionDto { Question = question.Question, Status = InvestigationQuestionStatuses.Undefined });
                            question.QuestionId = addedQuestion.Data.Id;
                        }
                    }
                }

                foreach (var item in model.InvestigationRecordParties.ToList())
                {
                    if (await _investigationRecordRepository.checkPartyExist(item.Id.Value, model.Id.Value))
                    {
                        model.InvestigationRecordParties.Remove(item);
                    }
                }
                await _investigationRecordRepository.EditAsync(model);

                await _attachmentService.UpdateListAsync(model.Attachments);


                return new ReturnResult<InvestigationRecordDto>(true, HttpStatuses.Status201Created, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<InvestigationRecordDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> RemoveAsync(int id)
        {
            try
            {
                await _investigationRecordRepository.RemoveAsync(id);

                return new ReturnResult<bool>(true, HttpStatuses.Status201Created, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }
    }
}
