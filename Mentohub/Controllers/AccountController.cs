using Mentohub.Core.Services.Services;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;


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
        
        private readonly UserService _userService;
        public AccountController(SignInManager<CurrentUser> signInManager, UserService userService,
           ILogger<AccountController> logger)
        {
           
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

            if (ModelState.IsValid && await _userService.CreateUser( model) != null)
            {
                return RedirectToAction("Index", "Home");

            }
            return View(model);
        }
        /// <summary>
        /// реєстрація користувача
        /// </summary>
        /// <param name="form"></param>
        
        /// <returns></returns>
        [HttpPost]
        [Route("createUser")]
        [SwaggerOperation(Summary = "Create a new user")]
        [SwaggerResponse(201, "User created successfully")]
        [SwaggerResponse(400, "Invalid input")]
        public JsonResult CreateNewUser(IFormCollection form)
        {
            string contentType = Request.ContentType;

            // Перевірка заголовка Content-Type
            //if (!contentType.StartsWith("multipart/form-data"))
            //{
            //    // Обробка помилки 415, якщо Content-Type не відповідає очікуваному
            //    return new JsonResult("Unsupported Media Type") { StatusCode = 415 };
            //}
            //var createdUser = _userService.CreateUser(form, model);
            return new JsonResult(string.Empty);            
            //return new JsonResult(createdUser)
            //{
            //    StatusCode = 201 // Код статусу "Created"
            //};
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
        public JsonResult LoginAsync([FromBody] LoginDTO credentials)
        {
            // Логіка аутентифікації користувача
            var authenticatedUser = _userService.Login(credentials);

            if (authenticatedUser != null)
            {
                return new JsonResult(authenticatedUser)
                {
                    StatusCode = 200
                };
            }
            else
            {
                return new JsonResult("Authentication failed")
                {
                    StatusCode = 401 // Код статусу "Unauthorized"
                };
            }
        }
        
    }
}