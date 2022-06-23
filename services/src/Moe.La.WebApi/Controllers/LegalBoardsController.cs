using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Common.Extensions;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/legal-boards")]
    [Authorize]

    public class LegalBoardsController : ControllerBase
    {
        private readonly ILegalBoardService _legalBoardService;

        public LegalBoardsController(ILegalBoardService legalBoardService)
        {
            _legalBoardService = legalBoardService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(LegalBoardQueryObject queryObject)
        {
            var result = await _legalBoardService.GetAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _legalBoardService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LegalBoardDto legalBoardDto)
        {
            var result = await _legalBoardService.AddAsync(legalBoardDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] LegalBoardDto legalBoardDto)
        {
            var result = await _legalBoardService.EditAsync(legalBoardDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _legalBoardService.RemoveAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpPost("changeStatus/{id}")]
        public async Task<IActionResult> ChangeStatusAsync(int id, int isActive)
        {
            var result = await _legalBoardService.ChangeStatusAsync(id, isActive);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("legal-board-type")]
        public IActionResult GetLegalBoardType()
        {
            return Ok(EnumExtensions.GetValues<LegalBoardTypes>());
        }

        [HttpGet("legal-boards")]
        public async Task<IActionResult> LegalBoards()
        {
            var result = await _legalBoardService.GetLegalBoard();
            return StatusCode((int)result.StatusCode, result);
        }

        #region legal-board-memo

        [HttpPost("legal-board-memo")]
        public async Task<IActionResult> CreateLegalBoardMemo([FromBody] LegalBoardMemoDto legalBoardMemoDto)
        {
            var result = await _legalBoardService.AddLegalBoardMemoAsync(legalBoardMemoDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("legal-board-memo")]
        public async Task<IActionResult> UpdateLegalBoardMemo([FromBody] LegalBoardMemoDto legalBoardMemoDto)
        {
            var result = await _legalBoardService.EditLegalBoardMemoAsync(legalBoardMemoDto);
            return StatusCode((int)result.StatusCode, result);
        }

        #endregion

        #region Legal-Board-members
        [HttpGet("legal-board-member-type")]
        public IActionResult GetLegalBoardMembershipType()
        {
            return Ok(EnumExtensions.GetValues<LegalBoardMembershipTypes>());
        }

        [HttpGet("legal-board-member-history")]
        public async Task<IActionResult> GetLegalBoardMemberHistory(LegalBoardMemberQueryObject queryObject)
        {
            var result = await _legalBoardService.GetLegalBoardMemberHistory(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpGet("legal-board-members")]
        public async Task<IActionResult> GetLegalBoardMembers(int boardId)
        {
            var result = await _legalBoardService.GetLegalBoardMembers(boardId);
            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region board-meeting
        [HttpGet("board-meeting")]
        public async Task<IActionResult> GetAllBoardMeeting(BoardMeetingQueryObject queryObject)
        {
            var result = await _legalBoardService.GetAllBoardMeetingAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("board-meeting/{id}")]
        public async Task<IActionResult> GetBoardMeeting(int id)
        {
            var result = await _legalBoardService.GetBoardMeetingAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpGet("board-meeting/{boardId}/{memoId}")]
        public async Task<IActionResult> GetMeetingByBoardAndMemo(int boardId, int memoId)
        {
            var result = await _legalBoardService.GetMeetingByBoardAndMemoAsync(boardId, memoId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("board-meeting")]
        public async Task<IActionResult> CreateBoardMeeting([FromBody] BoardMeetingDto boardMeetingDto)
        {
            var result = await _legalBoardService.AddBoardMeetingAsync(boardMeetingDto);
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpPut("board-meeting")]
        public async Task<IActionResult> UpdateBoardMeeting([FromBody] BoardMeetingDto boardMeetingDto)
        {
            var result = await _legalBoardService.EditBoardMeetingAsync(boardMeetingDto);
            return StatusCode((int)result.StatusCode, result);
        }



        #endregion
    }
}