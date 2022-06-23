using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moe.La.Core.Constants;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/attachments")]
    [Authorize]
    public class AttachmentController : ControllerBase
    {
        private readonly FileSettings _fileSettings;
        private readonly PhotoSettings _photoSettings;
        private readonly IAttachmentService _attachmentService;
        public AttachmentController(IAttachmentService attachmentService, IOptionsSnapshot<FileSettings> options, IOptionsSnapshot<PhotoSettings> optionsPhoto)
        {
            _fileSettings = options.Value;
            _photoSettings = optionsPhoto.Value;
            _attachmentService = attachmentService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll(AttachmentQueryObject queryObjectDto)
        {
            var result = await _attachmentService.GetAllAsync(queryObjectDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(AttachmentDto attachment)
        {
            if (attachment.File == null) return BadRequest("Null file");
            if (attachment.File.Length > _fileSettings.MaxBytes) return BadRequest("Max file size exceeded");
            if (!_fileSettings.IsSupported(attachment.File.FileName)) return BadRequest("Invalid file type.");

            var result = await _attachmentService.AddAsync(attachment);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _attachmentService.RemoveAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("download/{id}/{name}")]
        public IActionResult Download(Guid id, string name)
        {
            var fileName = id.ToString() + Path.GetExtension(name);
            var filePath = Path.Combine(ApplicationConstants.UploadLocation, fileName);
            var fileStream = new FileStream(filePath,
                  FileMode.Open,
                  FileAccess.Read
            );
            return new FileStreamResult(fileStream, "application/octet-stream") { FileDownloadName = name };
        }

        [AllowAnonymous]
        [HttpPut("test")]
        public Task Test()
        {
            _attachmentService.Cleanup();
            return Task.CompletedTask;
        }
    }
}