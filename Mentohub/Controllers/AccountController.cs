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
        [SwaggerOperation(Summary = "Реєстрація користувача", Tags = new[] { "Теги" })]
        public async Task<IActionResult> Register(IFormCollection form, RegisterDTO model)
        {

            if (ModelState.IsValid && await _userService.CreateUser(form, model) !=null)
            {
                return RedirectToAction("Index", "Home");

            }
            return View(model);
        }
        /// <summary>
        /// реєстрація користувача
        /// </summary>
        /// <param name="form"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("createUser")]
        [SwaggerOperation(Summary = "Create a new user")]
        [SwaggerResponse(201, "User created successfully")]
        [SwaggerResponse(400, "Invalid input")]
        public JsonResult CreateNewUser(IFormCollection form, RegisterDTO model)
        {
            var createdUser = _userService.CreateUser(form, model);
            return new JsonResult(createdUser)
            {
                StatusCode = 201 // Код статусу "Created"
            };
        }
        /// <summary>
        /// вхід в аккаунт
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("signin")]
        [SwaggerOperation(Summary = "Sign in a user")]
        [SwaggerResponse(200, "User signed in successfully")]
        [SwaggerResponse(401, "Authentication failed")]
        public JsonResult SignInUser([FromBody] LoginDTO credentials)
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
        [HttpDelete]
        [Route("deleteUser")]
        [SwaggerOperation(Summary = "Delete a user by ID")]
        [SwaggerResponse(204, "User deleted successfully")]
        [SwaggerResponse(404, "User not found")]
        public JsonResult DeleteUser([FromRoute] string userId)
        {
            // Логіка видалення користувача
            var deletedUser = _userService.DeleteUser(userId);

            if (deletedUser != null)
            {
                return new JsonResult(null)
                {
                    StatusCode = 204 // Код статусу "No Content"
                };
            }
            else
            {
                return new JsonResult("User not found")
                {
                    StatusCode = 404 // Код статусу "Not Found"
                };
            }
        }
        [HttpPut]
        [Route("updateUser")]
        [SwaggerOperation(Summary = "Update user information by ID")]
        [SwaggerResponse(200, "User updated successfully")]
        [SwaggerResponse(400, "Invalid input")]
        [SwaggerResponse(404, "User not found")]
        public JsonResult UpdateUser([FromRoute] string userId, IFormFile avatarFile, [FromBody] EditUserDTO user)
        {
            // Логіка оновлення інформації про користувача
            var updatedUser = _userService.UpdateUser(avatarFile, user);

            if (updatedUser != null)
            {
                return new JsonResult(updatedUser)
                {
                    StatusCode = 200 //профіль користувача успішно оновлено
                };
            }
            else
            {
                return new JsonResult("User not found")
                {
                    StatusCode = 404 // Код статусу "Not Found", користувача не знайдено
                };
            }
        }
        [HttpGet]
        [Route("getUser")]
        [SwaggerOperation(Summary = "Get user information by ID")]
        [SwaggerResponse(200, "User information retrieved successfully")]
        [SwaggerResponse(404, "User not found")]
        public JsonResult GetUser([FromRoute] string userId)
        {
            // Логіка отримання інформації про користувача
            var user = _userService.GetProfile(userId);

            if (user != null)
            {
                return new JsonResult(user);
            }
            else
            {
                return new JsonResult("User not found")
                {
                    StatusCode = 404 // Код статусу "Not Found"
                };
            }
        }
    }
}