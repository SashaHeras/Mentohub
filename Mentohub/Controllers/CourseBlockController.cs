using Mentohub.Core.Services.Interfaces;
using Mentohub.Core.Services.Services;
using Mentohub.Domain.Data.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Amqp.Framing;

namespace Mentohub.Controllers
{
    public class CourseBlockController : Controller
    {
        private readonly ICourseBlockService _courseBlockService;

        public CourseBlockController(
            ICourseBlockService courseBlockService
        )
        {
            _courseBlockService = courseBlockService;
        }

        /// <summary>
        /// Edit/create course block
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Edit([FromBody] CourseBlockDTO block)
        {
            try
            {
                var courseBlock = _courseBlockService.Edit(block);
                return Json(new { IsError = false, Data = courseBlock, Message = "Success" });
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true, Message = ex.Message });
            }
        }
    }
}
