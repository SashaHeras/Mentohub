using Mentohub.Core.Services.Services;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Mentohub.Core.AllExceptions;

namespace Mentohub.Core.Repositories.Repositories
{
    [Route("api/account")]
    [ApiController]
    [SwaggerTag("AccountController")]
    public class AccountController : Controller
    {
        private readonly SignInManager<CurrentUser> _signInManager;

        //private readonly IUserStore<CurrentUser> _userStore;
        //private readonly IUserEmailStore<CurrentUser> _emailStore;
        private readonly ILogger<AccountController> _logger;
        private readonly AllException _exception;
        private readonly UserService _userService;
        public AccountController(SignInManager<CurrentUser> signInManager, UserService userService,
           ILogger<AccountController> logger, AllException exception)
        {
            _exception = exception;
            _signInManager = signInManager;
            _logger = logger;
            _userService = userService;
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
                //return RedirectToAction("Index", "Home");
                return new JsonResult(createdUser)
                {
                    StatusCode=200
                };
              }

            }            
            catch (Exception ex) 
            { // Обробка помилки та повернення JsonResult із відповідними даними про помилку
                var errorResponse = new
                {
                    message = "Помилка при реєстрації користувача",
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
                var authenticatedUser =await _userService.Login(credentials);

               if (ModelState.IsValid && authenticatedUser != null)
               {
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
                    error = ex.Message // інформація про помилку
                };
                return new JsonResult("Authentication failed")
                {
                    StatusCode = 401 // Код статусу "Unauthorized"
                };
            }
            return _exception.NotFoundObjectResult("User is not found");
        }
        public JsonResult LogoutAsync()
        {
            return new JsonResult(_userService.LogOut());
        }
        
    }
}