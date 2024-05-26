using Acme.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Acme.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptsController(IOcrService ocrService) : ControllerBase
    {
        private readonly IOcrService _ocrService = ocrService;

        [HttpPost("parse")]
        public async Task<IActionResult> UploadReceipt([Required] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var permittedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !Array.Exists(permittedExtensions, e => e == ext))
                return BadRequest("Invalid file type.");

            try
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();

                // Call the service layer to process the receipt image
                var response = await _ocrService.ParseReceiptAsync(fileBytes);

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log the exception
                // _logger.LogError(ex, "Error uploading receipt image.");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}