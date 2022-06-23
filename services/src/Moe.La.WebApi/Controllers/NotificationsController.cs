using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Core.Dtos;
using Moe.La.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/notifications")]
    [Authorize]

    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAll(NotificationSystemQueryObject queryObject)
        //{
        //    var result = await _notificationService.GetAllAsync(queryObject);
        //    return StatusCode((int)result.StatusCode, result);
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> Get(int id)
        //{
        //    var result = await _notificationService.GetAsync(id);
        //    return StatusCode((int)result.StatusCode, result);
        //}

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NotificationDto notificationDto)
        {
            var result = await _notificationService.AddAsync(notificationDto);
            return StatusCode((int)result.StatusCode, result);
        }

        //[HttpPut]
        //public async Task<IActionResult> Update([FromBody] NotificationDto notificationDto)
        //{
        //    var result = await _notificationService.EditAsync(notificationDto);
        //    return StatusCode((int)result.StatusCode, result);
        //}

        //[HttpPut("read/{id}")]
        //public async Task<IActionResult> Read(int id)
        //{
        //    var result = await _notificationService.ReadAsync(id);
        //    return StatusCode((int)result.StatusCode, result);
        //}
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var result = await _notificationService.RemoveAsync(id);
        //    return StatusCode((int)result.StatusCode, result);
        //}
    }
}
