using Microsoft.Extensions.Logging;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using Moe.La.ServiceInterface.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class RequestService : IRequestService
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IRequestTransactionRepository _requestTransactionRepository;
        private readonly IUserRepository _userRepository;

        private readonly ILogger<RequestService> _logger;

        public RequestService(IRequestRepository requestRepository, IRequestTransactionRepository requestTransactionRepository,
            IUserRepository userRepository, ILogger<RequestService> logger)
        {
            _requestRepository = requestRepository;
            _requestTransactionRepository = requestTransactionRepository;
            _userRepository = userRepository;

            _logger = logger;
        }

        public async Task<ReturnResult<QueryResultDto<RequestListItemDto>>> GetAllAsync(RequestQueryObject queryObject)
        {
            try
            {
                var entities = await _requestRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<RequestListItemDto>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entities
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, queryObject);

                return new ReturnResult<QueryResultDto<RequestListItemDto>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<ReturnResult<RequestListItemDto>> GetAsync(int id)
        {
            try
            {
                var entitiy = await _requestRepository.GetAsync(id);

                return new ReturnResult<RequestListItemDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = entitiy
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<RequestListItemDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }

        public async Task<List<UserDetailsDto>> GetRequestApproveUsers(int requestId)
        {
            try
            {
                var usersSignatures = new List<UserDetailsDto>();


                var transactions = await _requestTransactionRepository.GetAllAsync(
                    new TransactionQueryObject() { RequestId = requestId });

                var approveTransactions = transactions.Items.ToList();

                foreach (var transaction in approveTransactions)
                { 
                    // no user repeat (researcher)
                    if (!usersSignatures.Select(u => u.Id).Contains(Guid.Parse(transaction.CreatedById)))
                    {
                        var user = await _userRepository.GetAsync(Guid.Parse(transaction.CreatedById));
                        usersSignatures.Add(user);
                    }
                }
                 
                return usersSignatures;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, requestId);

                return null;
            }
        }

        public async Task<RequestForPrintDto> GetForPrintAsync(int id)
        {
            try
            {
                var entity = await _requestRepository.GetForPrintAsync(id);

                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return null;
            }
        }
        public async Task<bool> UpdateStatusAsync(int id, RequestStatuses status)
        {
            try
            {
                await _requestRepository.UpdateStatusAsync(id, status);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return false;
            }
        }


        public async Task<ReturnResult<RequestTransactionDto>> AddTransactionAsync(RequestTransactionDto requestTransactionDto)
        {
            try
            {
                var errors = new List<string>();

                var validationResult = ValidationResult.CheckModelValidation(new RequestTransactionValidator(), requestTransactionDto);
                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }
                await _requestTransactionRepository.AddAsync(requestTransactionDto);

                return new ReturnResult<RequestTransactionDto>(true, HttpStatuses.Status200OK, requestTransactionDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, requestTransactionDto);

                return new ReturnResult<RequestTransactionDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }



    }
}
