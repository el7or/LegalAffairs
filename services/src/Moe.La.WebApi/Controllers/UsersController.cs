using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Services;
using System;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly PhotoSettings _photoSettings;

        public UsersController(IUserService userService, IOptionsSnapshot<PhotoSettings> optionsPhoto)
        {
            _userService = userService;
            _photoSettings = optionsPhoto.Value;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(UserQueryObject queryObject)
        {
            var result = await _userService.GetAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _userService.GetAsync(id);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserDto userDto)
        {
            var result = await _userService.AddAsync(userDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UserDto userDto)
        {
            var result = await _userService.EditAsync(userDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.RemoveAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("roles/{id}")]
        public async Task<IActionResult> GetRoles(Guid id)
        {
            var result = await _userService.GetUserRolesAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        //[HttpPost("upload-photo/{userId}")]
        //public async Task<IActionResult> Create(Guid userId, IFormFile file)
        //{

        //    if (file == null) return BadRequest("Null file");
        //    if (file.Length == 0) return BadRequest("Empty file");

        //    if (file.Length > _photoSettings.MaxBytes) return BadRequest("Max file size exceeded");
        //    if (!_photoSettings.IsSupported(file.FileName)) return BadRequest("Invalid file type.");


        //    var result = await _userService.AddUserPhotoAsync(file);
        //    return StatusCode((int)result.StatusCode, result);

        //}


        [HttpGet("consultants")]
        public async Task<IActionResult> GetConsultants(string name, int? legalBoardId)
        {
            var result = await _userService.GetConsultants(name, legalBoardId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("{userId}/enabled/{enable}")]
        public async Task<IActionResult> EnabledUser(Guid userId, bool enable)
        {
            var result = await _userService.EnabledUserAsync(userId, enable);
            return StatusCode((int)result.StatusCode, result);
        }



    }
}