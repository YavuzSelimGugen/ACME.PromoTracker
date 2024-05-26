using Acme.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Acme.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCodesController(IProductCodeService productCodeService) : ControllerBase
    {
        private readonly IProductCodeService _productCodeService = productCodeService;

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateProductCode()
        {
            var code = _productCodeService.GenerateUniqueCode();
            return Ok(code);
        }

        [HttpGet("{code}/validate")]
        public async Task<IActionResult> ValidateProductCode(string code)
        {
            var isValid = _productCodeService.ValidateCode(code);
            return Ok(isValid);
        }
    }
}