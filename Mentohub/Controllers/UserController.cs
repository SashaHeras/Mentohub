using Mentohub.Core.AllExceptions;
using Mentohub.Core.Repositories.Interfaces;
using Mentohub.Core.Repositories.Interfaces.PaymentInterfaces;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.DTO.Payment;
using Mentohub.Domain.Helpers;
using Mentohub.Domain.Mappers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Swashbuckle.AspNetCore.Annotations;

namespace Mentohub.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/user")]
    [ApiController]
    [SwaggerTag("UserController")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly AllException _exception;
        private readonly IUserService _userService;
        private readonly ICRUD_UserRepository _cRUD;
        private readonly ICourseService _courseService;
        private readonly IOrderRepository _orderRepository;
        private readonly IAzureService _azureService;

        public UserController(
            ILogger<UserController> logger, 
            IUserService userService,
            AllException exception, 
            ICRUD_UserRepository cRUD_UserRepository,
            ICourseService courseService,
            IOrderRepository orderRepository
        )
        {
            _logger = logger;
            _userService = userService;
            _exception = exception;
            _cRUD = cRUD_UserRepository;
            _courseService = courseService;
            _orderRepository = orderRepository;
        }
        
        [HttpDelete]
        [Route("deleteUser")]
        [SwaggerOperation(Summary = "Delete a user by ID")]        
        public async Task<IActionResult> DeleteUser([FromForm] string userId)
        {
            // Логіка видалення користувача
            try 
            { 
                var deletedUser =await _userService.DeleteUser(userId);
                if (deletedUser)
                {
                    _logger.LogInformation("User deleted successfully.");
                    var result= Json(new { IsError = false, Message = "User deleted successfully " });
                    result.StatusCode = 200;
                    return result;
                }
                else
                {
                    var result = Json(new { IsError = true, Message = "User not found"});
                    result.StatusCode = 404;
                    return result;                 
                }
            }
            catch(Exception ex)
            {
                var errorResponse = new
                {
                    message = "Error when deleting a user",
                    error = ex.Message // інформація про помилку
                };
                var result = Json(new { IsError = true, errorResponse});
                result.StatusCode = 500;
                return result;
            }   
        }

        /// <summary>
        /// отримання профіля користувача
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getUserProfile")]
        [SwaggerOperation(Summary = "Get user profile")]
        public async Task<IActionResult> GetUserProfile([FromForm] string id)
        {
            try
            {
                var profile = await _userService.GetProfile(id);               
                if(profile == null)
                {
                    return new JsonResult(("Not Found"))
                    {
                        StatusCode = 404
                    }; 
                }
                return new JsonResult(profile)
                {
                    StatusCode = 200
                };
            }
            catch(Exception ex) 
            {
                var errorResponse = new
                {
                    message = "Error when getting a user",
                    error = ex.Message // інформація про помилку
                };
                return new JsonResult(errorResponse)
                {
                    StatusCode = 500 // код статусу, що вказує на помилку
                };
            }            
        }

        /// <summary>
        /// оновлення інформації про користувача
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="avatarFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("updateUser")]
        [SwaggerOperation(Summary = "Update user information by ID")]
        [SwaggerResponse(200, "User updated successfully")]
        [SwaggerResponse(400, "Invalid input")]
        [SwaggerResponse(404, "User not found")]
        public async Task<JsonResult> UpdateUser([FromForm] UserDTO userDTO)
        {
            try
            {               
                // Логіка оновлення інформації про користувача
                var updatedUser = await _userService.UpdateUser(userDTO);
                if (!updatedUser)
                {
                    var result400 = Json(new { IsError = true, Message = "Invalid input", StatusCode = 400 });
                    result400.StatusCode = 400;
                    return result400;
                    
                }
                var result = Json(new { IsError = false, Message = "User updated successfully", StatusCode = 200 });
                result.StatusCode = 200;
                return result;
            }
            catch(Exception ex)
            {
                var errorResponse = new
                {
                    message = "Error when updating a user",
                    error = ex.Message // інформація про помилку
                };
                var result = Json(new { IsError = true, errorResponse, StatusCode = 500 });
                result.StatusCode = 500;
                return result;
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="avatar"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("uploadAvatar")]
        [SwaggerOperation(Summary = "Download user's avatar")]
        public async Task<JsonResult> UploadAvatar([FromForm] IFormFile avatar, [FromForm] string id)
        {
            try
            { 
                var avatarUrl = await _userService.UploadAvatar(avatar, id);
                if(avatarUrl != null)
                {
                    return Json(new { IsError = false, avatarUrl });                      
                }
                return Json(new { IsError = true, Message = "Not file for download" });                
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    message = "Error when was a download of avatar",
                    error = ex.Message // інформація про помилку
                };

                return Json(errorResponse);                
            }
        }

        /// <summary>
        /// додавання ролі користувачу
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("addRole")]
        [SwaggerOperation(Summary ="add role to user`s roles")]
        public async Task<JsonResult> AddUserRoles([FromForm]string userId,
            [FromForm]string roleId)
        {
            try
            {              
                if( await _userService.AddRoleToUserListRoles(userId, roleId))
                {
                    var UserDTO = await _userService.GetProfile(userId);
                    var result= Json(new { IsError = false, UserDTO ,StatusCode=200});
                    result.StatusCode = 200;
                    return result;
                }
                var resultError= Json(new { IsError = true, Message = "Add role is failed",StatusCode=404 });
                resultError.StatusCode = 404;
                return resultError;
            }
            catch(Exception ex)
            {
                var errorResponse = new
                {
                    message = "Error when add user`s role",
                    error = ex.Message // інформація про помилку
                };
                var result= Json(new { IsError = true, errorResponse,StatusCode=500 });
                result.StatusCode = 500;
                return result;
            }           
        }

        /// <summary>
        /// отримання аватарки
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getAvatar")]
        public async Task<IActionResult> GetAvatar(string id)
        {
            // Отримайте URL аватарки користувача з сервісу
            string avatarUrl = await _userService.GetAvatarUrl(id);

            if (!string.IsNullOrEmpty(avatarUrl))
            {
                var result200 = Json(new { IsError = false, Message = avatarUrl, StatusCode = 200 });
                result200.StatusCode = 200;
                return result200;
            }

            // Якщо аватарка не знайдена або виникла помилка
            var result = Json(new { success = false, message = "Error when getting an avatar",StatusCode=404 });
            result.StatusCode = 404;
            return result;
            
        }

        [Route("GetEncryptedUserID")]
        [HttpPost]
        public JsonResult ShyfrUserID([FromForm] string userID)
        {
            return Json(MentoShyfr.Encrypt(userID));
        }

        [HttpPost]
        [Route("AddRoleAuthorToCurrentUser")]
        public async Task<JsonResult> AddToUserRoleAuthor([FromForm] string userId)
        {
            try
            {
                var result = await _userService.AddRoleAuthor(userId);
                var jsonResult= Json(new { IsError = false, Message = "Success", StatusCode = 200 });
                jsonResult.StatusCode = 200;
                return jsonResult;
            }

            catch (AllException ex)
            {
                var result= Json(new
                {
                    IsError = true,
                    NeedFillFields = true,
                    Message = ex.NotificationMessage("Please fill out the required fields!")
                });
                result.StatusCode = 404;
                return result;
            }
            catch (Exception ex)
            {
                var result = Json(new { IsError = true, Message = ex.Message, StatusCode = 500 });
                result.StatusCode = 500;
                return result;
            }
        }

        [HttpPost]
        [Route("GetAuthorsCourses")]
        public JsonResult GetAuthorsCourses([FromForm] string authorID)
        {
            try
            {
                var result = _courseService.GetAuthorsCourses(authorID); ;
                return Json(new { IsError = false, Data = result, Message = "Success" });
            }
            catch(Exception ex)
            {
                return Json(new { IsError = true, Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("GetActiveOrder")]
        public JsonResult GetActiveOrder([FromForm] string authorID)
        {
            var orderDTO = new OrderDTO();
            var currentUser = MentoShyfr.Decrypt(authorID);
            var order = _orderRepository.GetAll(x => x.Ordered == null && x.UserID == currentUser).FirstOrDefault();

            if(order == null)
            {
                return Json(null);
            }

            orderDTO = new OrderDTO()
            {
                ID = order.ID,
                Created = order.Created,
                Total = order.Total,
                UserID = order.UserID,
                Items = order.OrderItems.Select(x => new OrderItemDTO()
                {
                    ID = x.ID,
                    CourseID = x.CourseID,
                    Course = CourseMapper.ToDTO(x.Course),
                    Total = x.Total,
                    Pos = x.Pos,
                    Price = x.Price
                }).ToList()
            };

            return Json(orderDTO);
        }
    }
}


