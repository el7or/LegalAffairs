using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/system-notifications")]
    [Authorize]
    public class NotificationsSystemController : ControllerBase
    {
        private readonly INotificationSystemService _notificationSystemService;

        public NotificationsSystemController(INotificationSystemService NotificationsSystemervice)
        {
            _notificationSystemService = NotificationsSystemervice;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(NotificationSystemQueryObject queryObject)
        {
            var result = await _notificationSystemService.GetAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _notificationSystemService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("read/{id}")]
        public async Task<IActionResult> Read(int id)
        {
            var result = await _notificationSystemService.ReadAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }



    }
}
