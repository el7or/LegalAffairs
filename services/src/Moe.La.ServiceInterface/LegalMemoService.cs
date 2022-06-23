using Microsoft.Extensions.Logging;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using Moe.La.ServiceInterface.Validators.Others;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class LegalMemoService : ILegalMemoService
    {
        private readonly ILegalMemoRepository _legalMemoRepository;
        private readonly ILegalMemosHistoryRepository _legalMemosHistoryRepository;
        private readonly ILegalMemoNoteRepository _legalMemoNoteRepository;
        private readonly ILogger<LegalMemoService> _logger;

        public LegalMemoService(ILegalMemoRepository legalMemoRepository, ILegalMemosHistoryRepository legalMemosHistoryRepository,
            ILegalMemoNoteRepository legalMemoNoteRepository, ILogger<LegalMemoService> logger)
        {
            _legalMemoRepository = legalMemoRepository;
            _legalMemosHistoryRepository = legalMemosHistoryRepository;
            _legalMemoNoteRepository = legalMemoNoteRepository;
            _logger = logger;
        }

        #region LegalMemo
        public async Task<ReturnResult<QueryResultDto<LegalMemoListItemDto>>> GetAllAsync(LegalMemoQueryObject queryObject)
        {
            try
            {
                var entities = await _legalMemoRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<LegalMemoListItemDto>>(true, HttpStatuses.Status200OK, entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, queryObject);

                return new ReturnResult<QueryResultDto<LegalMemoListItemDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<LegalMemoDetailsDto>> GetAsync(int id)
        {
            try
            {
                var entity = await _legalMemoRepository.GetAsync(id);

                if (entity == null)
                {
                    return new ReturnResult<LegalMemoDetailsDto>(false, HttpStatuses.Status404NotFound, new List<string> { "البيانات المطلوبة غير متوفرة" });
                }

                return new ReturnResult<LegalMemoDetailsDto>(true, HttpStatuses.Status200OK, entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<LegalMemoDetailsDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }
        public async Task<ReturnResult<LegalMemoForPrintDetailsDto>> GetForPrintAsync(int id, int hearingId)
        {
            try
            {
                var entity = await _legalMemoRepository.GetForPrintAsync(id, hearingId);

                if (entity == null)
                {
                    return new ReturnResult<LegalMemoForPrintDetailsDto>(false, HttpStatuses.Status404NotFound, new List<string> { "البيانات المطلوبة غير متوفرة" });
                }

                return new ReturnResult<LegalMemoForPrintDetailsDto>(true, HttpStatuses.Status200OK, entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<LegalMemoForPrintDetailsDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<LegalMemoDto>> AddAsync(LegalMemoDto legalMemoDto)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new LegalMemoValidator(), legalMemoDto);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<LegalMemoDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                await _legalMemoRepository.AddAsync(legalMemoDto);

                return new ReturnResult<LegalMemoDto>(true, HttpStatuses.Status201Created, legalMemoDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, legalMemoDto);

                return new ReturnResult<LegalMemoDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<LegalMemoDto>> EditAsync(LegalMemoDto legalMemoDto)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new LegalMemoValidator(), legalMemoDto);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<LegalMemoDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                var entityToHistory = await _legalMemoRepository.GetToHistoryAsync(legalMemoDto.Id);
                await _legalMemosHistoryRepository.AddAsync(entityToHistory);
                await _legalMemoRepository.EditAsync(legalMemoDto);

                return new ReturnResult<LegalMemoDto>(true, HttpStatuses.Status200OK, legalMemoDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, legalMemoDto);

                return new ReturnResult<LegalMemoDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> RemoveAsync(DeletionLegalMemoDto deletionLegalMemoDto)
        {
            try
            {
                await _legalMemoRepository.RemoveAsync(deletionLegalMemoDto);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, deletionLegalMemoDto);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> ChangeLegalMemoStatusAsync(int legalMemoId, int legalMemoStatusId)
        {
            try
            {
                //get all details of legal memo that not exist in legalMemoDetails
                var entityToHistory = await _legalMemoRepository.GetToHistoryAsync(legalMemoId);
                await _legalMemosHistoryRepository.AddAsync(entityToHistory);

                if (legalMemoStatusId == (int)LegalMemoStatuses.Approved
                    || legalMemoStatusId == (int)LegalMemoStatuses.Returned
                    || legalMemoStatusId == (int)LegalMemoStatuses.Accepted
                    || legalMemoStatusId == (int)LegalMemoStatuses.RaisingSubBoardHead
                    || legalMemoStatusId == (int)LegalMemoStatuses.RaisingMainBoardHead
                    || legalMemoStatusId == (int)LegalMemoStatuses.RaisingAllBoardsHead)
                {
                    await _legalMemoNoteRepository.CloseCurrentNotesAsync(legalMemoId);
                }

                await _legalMemoRepository.ChangeLegalMemoStatusAsync(legalMemoId, legalMemoStatusId);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }


        public async Task<ReturnResult<bool>> RaisToAllBoardsHeadAsync(int id)
        {
            try
            {
                //get all details of legal memo that not exist in legalMemoDetails
                var entityToHistory = await _legalMemoRepository.GetToHistoryAsync(id);
                await _legalMemosHistoryRepository.AddAsync(entityToHistory);


                await _legalMemoNoteRepository.CloseCurrentNotesAsync(id);


                await _legalMemoRepository.ChangeLegalMemoStatusAsync(id, (int)LegalMemoStatuses.RaisingAllBoardsHead);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> ApproveAsync(int id)
        {
            try
            {
                //get all details of legal memo that not exist in legalMemoDetails
                var entityToHistory = await _legalMemoRepository.GetToHistoryAsync(id);
                await _legalMemosHistoryRepository.AddAsync(entityToHistory);


                await _legalMemoNoteRepository.CloseCurrentNotesAsync(id);


                await _legalMemoRepository.ChangeLegalMemoStatusAsync(id, (int)LegalMemoStatuses.Approved);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> RejectAsync(int id, string note)
        {
            try
            {
                //get all details of legal memo that not exist in legalMemoDetails
                var entityToHistory = await _legalMemoRepository.GetToHistoryAsync(id);
                await _legalMemosHistoryRepository.AddAsync(entityToHistory);


                // add reject note
                int reviewNumber = await _legalMemoNoteRepository.GetCurrentReviewNumberAsync(id);

                await _legalMemoNoteRepository.AddAsync(new LegalMemoNoteDto { LegalMemoId = id, Text = note, IsClosed = true, ReviewNumber = reviewNumber });


                await _legalMemoRepository.ChangeLegalMemoStatusAsync(id, (int)LegalMemoStatuses.Rejected);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }


        public async Task<ReturnResult<bool>> ReadLegalMemoAsync(int legalMemoId)
        {
            try
            {
                var entityToHistory = await _legalMemoRepository.GetToHistoryAsync(legalMemoId);
                await _legalMemosHistoryRepository.AddAsync(entityToHistory);

                // Get review number
                int reviewNumber = await _legalMemoNoteRepository.GetCurrentReviewNumberAsync(legalMemoId);
                await _legalMemoRepository.ReadLegalMemoAsync(legalMemoId, reviewNumber);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }
        public async Task<ReturnResult<ICollection<LegalMemoListItemDto>>> GetAllLegalMemoByCaseIdAsync(int CaseID)
        {
            try
            {
                var entities = await _legalMemoRepository.GetAllLegalMemoByCaseIdAsync(CaseID);

                return new ReturnResult<ICollection<LegalMemoListItemDto>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entities
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, CaseID);

                return new ReturnResult<ICollection<LegalMemoListItemDto>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        #endregion

        #region LegalMemoNotes

        public async Task<ReturnResult<QueryResultDto<LegalMemoNoteListItemDto>>> GetNotesAllAsync(LegalMemoNoteQueryObject queryObject)
        {
            try
            {
                var entities = await _legalMemoNoteRepository.GetAllAsync(queryObject);

                if (entities == null)
                {
                    return new ReturnResult<QueryResultDto<LegalMemoNoteListItemDto>>(false, HttpStatuses.Status404NotFound, new List<string> { "البيانات المطلوبة غير متوفرة" });
                }

                return new ReturnResult<QueryResultDto<LegalMemoNoteListItemDto>>(true, HttpStatuses.Status200OK, entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, queryObject);

                return new ReturnResult<QueryResultDto<LegalMemoNoteListItemDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<LegalMemoNoteListItemDto>> GetNoteAsync(int id)
        {
            try
            {
                var entity = await _legalMemoNoteRepository.GetAsync(id);

                if (entity == null)
                {
                    return new ReturnResult<LegalMemoNoteListItemDto>(false, HttpStatuses.Status404NotFound, new List<string> { "البيانات المطلوبة غير متوفرة" });
                }

                return new ReturnResult<LegalMemoNoteListItemDto>(true, HttpStatuses.Status200OK, entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<LegalMemoNoteListItemDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<LegalMemoNoteDto>> AddNoteAsync(LegalMemoNoteDto legalMemoNoteDto)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new LegalMemoNoteValidator(), legalMemoNoteDto);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<LegalMemoNoteDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                await _legalMemoNoteRepository.AddAsync(legalMemoNoteDto);

                return new ReturnResult<LegalMemoNoteDto>(true, HttpStatuses.Status200OK, legalMemoNoteDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, legalMemoNoteDto);

                return new ReturnResult<LegalMemoNoteDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<LegalMemoNoteDto>> EditNoteAsync(LegalMemoNoteDto legalMemoNoteDto)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new LegalMemoNoteValidator(), legalMemoNoteDto);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<LegalMemoNoteDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                await _legalMemoNoteRepository.EditAsync(legalMemoNoteDto);

                return new ReturnResult<LegalMemoNoteDto>(true, HttpStatuses.Status200OK, legalMemoNoteDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, legalMemoNoteDto);

                return new ReturnResult<LegalMemoNoteDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> RemoveNoteAsync(int id)
        {
            try
            {
                await _legalMemoNoteRepository.RemoveAsync(id);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        #endregion

        #region LegalMemosHistory

        public async Task<ReturnResult<QueryResultDto<LegalMemosHistoryListItemDto>>> GetHistoryAllAsync(LegalMemoQueryObject queryObject)
        {
            try
            {
                var entities = await _legalMemosHistoryRepository.GetAllAsync(queryObject);

                if (entities == null)
                {
                    return new ReturnResult<QueryResultDto<LegalMemosHistoryListItemDto>>(false, HttpStatuses.Status404NotFound, new List<string> { "البيانات المطلوبة غير متوفرة" });
                }

                return new ReturnResult<QueryResultDto<LegalMemosHistoryListItemDto>>(true, HttpStatuses.Status200OK, entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, queryObject);

                return new ReturnResult<QueryResultDto<LegalMemosHistoryListItemDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        #endregion
    }
}
