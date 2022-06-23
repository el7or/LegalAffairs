using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Core.Dtos.Consultations;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/consultation-transaction")]
    [Authorize]
    public class ConsultationTransactionsController : ControllerBase
    {
        private readonly IConsultationTransactionService _transactionService;

        public ConsultationTransactionsController(IConsultationTransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ConsultationTransactionDto transactionDto)
        {
            var result = await _transactionService.AddAsync(transactionDto);

            return StatusCode((int)result.StatusCode, result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(ConsultationTransactionQueryObject queryObject)
        {
            var result = await _transactionService.GetAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
