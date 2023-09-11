using Azure;
using Microsoft.AspNetCore.Identity;
using ProductApi;
using ProductAPI.Data;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json.Serialization;

namespace ProductAPI
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<AccountService> _logger;
        private readonly ITokenService _tokenService;

        public AccountService(ApplicationDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ILogger<AccountService> logger, ITokenService tokenService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _tokenService = tokenService;
        }

        public async Task<Response<bool>> RegisterAsync(RegisterViewModel model)
        {
            try
            {
                var user = new IdentityUser
                {
                    Email = model.Email,
                    UserName = model.Email,
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    return new Response<bool> { Data= false, StatusCode = HttpStatusCode.BadRequest, Message = string.Join(", ", result.Errors.Select(v => v.Description)), Success = false };
                }
                return new Response<bool> { Data = true, StatusCode = HttpStatusCode.Created, Success = true };
            }
            catch (Exception e)
            {

                // Log the exception
                _logger.LogError(e.Message);
                return new Response<bool> { Data = false, StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred while registering", Success = false };

            }
        }
        public async Task<Response<string>> LoginAsync(RegisterViewModel model)
        {
            try
            {

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, lockoutOnFailure: true);

                if (!result.Succeeded)
                {
                    return new Response<string> { StatusCode = HttpStatusCode.BadRequest, Message = "Incorrect email or password", Success = false };

                }
                var token = _tokenService.GenerateAccessToken();
                return new Response<string> { Data = token, StatusCode = HttpStatusCode.OK, Success = true };
            }
            catch (Exception e)
            {

                // Log the exception
                _logger.LogError(e.Message);
                return new Response<string> { StatusCode = HttpStatusCode.InternalServerError, Message = "An error occurred", Success = false };
            }
        }
    }
    public class RegisterViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
    public class Response<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; }
    }

}
