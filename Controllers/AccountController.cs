using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Data;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Security.Cryptography;

namespace ProductAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _service;
        public AccountController(IAccountService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(RegisterViewModel model)
        {
            var result = await _service.RegisterAsync(model);

            return StatusCode((int)result.StatusCode, result);
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(RegisterViewModel model)
        {
            var result = await _service.LoginAsync(model);

            return StatusCode((int)result.StatusCode, result);
        }
    }
}
