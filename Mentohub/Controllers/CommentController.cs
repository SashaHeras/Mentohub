using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Helpers;
using Mentohub.Domain.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Mentohub.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        /// <summary>
        /// Get course comments list
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [Route("Comment/List")]
        [HttpPost]
        public JsonResult List([FromBody] CommentFilter filter)
        {
            var data = _commentService.List(filter);
            return Json(data);
        }

        /// <summary>
        /// Edit/create comment to course
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("/Comment/Edit")]
        [HttpPost]
        public JsonResult Edit([FromBody] CommentDTO data)
        {
            try
            {
                var comment = _commentService.Edit(data);
                return Json(new { IsError = false, Data = comment, Message = "Success" });
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true, Message = ex.Message });
            }
        }
    }
}
