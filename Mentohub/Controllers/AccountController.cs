using Mentohub.Core.Services.Services;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Mentohub.Core.AllExceptions;
using Microsoft.AspNetCore.Authorization;
using Mentohub.Core.Services;
using Microsoft.AspNetCore.SignalR;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Data.DTO.Helpers;

namespace Mentohub.Core.Repositories.Repositories
{
    [Route("api/account")]
    [ApiController]
    [SwaggerTag("AccountController")]
    public class AccountController : Controller
    {
        private readonly SignInManager<CurrentUser> _signInManager;

        private readonly ILogger<AccountController> _logger;
        private readonly AllException _exception;
        private readonly IUserService _userService;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<CurrentUser> _userManager;
        private readonly IHubContext<SignalRHub> _hubContext;

        public AccountController(
            SignInManager<CurrentUser> signInManager,
            IUserService userService,
            ILogger<AccountController> logger,
            AllException exception,
            IEmailSender emailSender,
            UserManager<CurrentUser> userManager, IHubContext<SignalRHub> hubContext)
        {
            _exception = exception;
            _signInManager = signInManager;
            _logger = logger;
            _userService = userService;
            _emailSender = emailSender;
            _userManager = userManager;
            _hubContext = hubContext;
        }

        [HttpGet]
        [Route("register")]
        [SwaggerOperation(Summary = "Отримання форми реєстрації користувача", Tags = new[] { "Теги" })]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("register")]
        [SwaggerOperation(Summary = "Реєстрація користувача", Tags = new[] { "Теги" })]
        public async Task<IActionResult> Register([FromForm] RegisterDTO model)
        {
            try
            {
                var createdUser = await _userService.CreateUser(model);
                if (ModelState.IsValid && createdUser != null)
                {
                    // Генеруємо токен для підтвердження email
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(createdUser);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account",
                        new { userId = createdUser.Id, code = code },
                        protocol: HttpContext.Request.Scheme);

                    // Відправляємо лист для підтвердження email
                    await _emailSender.SendEmailAsync(model.Email, "Confirm your email",
                        $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");

                    _logger.LogInformation("User created successfully.");
                    return new JsonResult(createdUser)
                    {
                        StatusCode = 200
                    };
                }
            }            
            catch (Exception ex) 
            { 
                // Обробка помилки та повернення JsonResult із відповідними даними про помилку
                var errorResponse = new
                {
                    message = "Error during user registration",
                    error = ex.Message // інформація про помилку
                };
                return new JsonResult(errorResponse)
                {
                    StatusCode = 500 // код статусу, що вказує на помилку
                };

            }

            return _exception.NotFoundObjectResult("User was not created");
        }
        
        /// <summary>
        /// вхід в аккаунт
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        [SwaggerOperation(Summary = "Sign in a user")]
        [SwaggerResponse(200, "User signed in successfully")]
        [SwaggerResponse(401, "Authentication failed")]
        public async Task<IActionResult> LoginAsync([FromForm] LoginDTO credentials)
        {
            // Логіка аутентифікації користувача
            try
            {
                var authenticatedUser = await _userService.Login(credentials);
                if (ModelState.IsValid && authenticatedUser != null)
                {
                    Response.Cookies.Append("userID", MentoShyfr.Encrypt(authenticatedUser.Id.ToString()));
                    return new JsonResult(authenticatedUser)
                    {
                        StatusCode = 200
                    };
                }
            }
            catch(Exception ex)
            {
                var errorResponse = new
                {
                    message = "Error trying to log in user",
                    error = ex.Message                              // інформація про помилку
                };

                return new JsonResult("Authentication failed")
                {
                    StatusCode = 401                                // Код статусу "Unauthorized"
                };
            }

            return _exception.NotFoundObjectResult("User is not found");
        }

        public JsonResult LogoutAsync()
        {
            return new JsonResult(_userService.LogOut());
        }

        /// <summary>
        /// Confirm user's email after registration.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet("ConfirmEmail")]
        [ProducesResponseType(typeof(JsonResult), 200)]
        [ProducesResponseType(typeof(JsonResult), 400)]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {            
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
            {
                return new JsonResult("No data to send")
                {
                    StatusCode = 400
                }; 
            }

            var user = await _userService.GetCurrentUser(userId);
            if (user == null)
            {
                return new JsonResult("User is not found")
                {
                    StatusCode = 400
                };
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                return new JsonResult("Email was not confirmed")
                {
                    StatusCode = 400
                };
            }

            return new JsonResult("Email was confirmed")
            {
                StatusCode = 200
            };
        }
    }
}