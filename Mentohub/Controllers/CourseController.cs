using Mentohub.Core.Context;
using Mentohub.Core.Services.Services;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Entities;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Mentohub.Controllers
{
    public class CourseController : Controller
    {
        private readonly ProjectContext _context;

        private readonly CourseService _courseService;
        private readonly LessonService _lessonService;
        private readonly MediaService _mediaService;
        private readonly CourseItemService _courseItemService;

        public CourseController(ProjectContext projectContext, 
            CourseService service, 
            LessonService lessonService,
            MediaService mediaService, 
            CourseItemService courseItemService)
        {
            _context = projectContext;
            _courseService = service;  
            _courseItemService = courseItemService;
            _lessonService = lessonService;
            _mediaService = mediaService;
        }

        public IActionResult Index(int id)
        {
            List<CourseElement>? elements = _courseService.GetCourseElements(id);
            ViewBag.CourseId = id;           
            
            if(elements != null)
            {
                ViewBag.DefaultTypeId = elements[0].TypeId;
                ViewBag.DefaultCourseItemId = elements[0].CourseItemId;
            }

            return View();
        }

        [Route("/Course/CreateCourse/{id}")]
        public IActionResult CreateCourse(int id)
        {
            ViewBag.Subjects = _context.CourseSubjects.ToList();
            if (id == 0)
            {
                ViewBag.Course = _courseService.CreateNewCourse();
            }
            else
            {
                ViewBag.Course = _courseService.GetCourse(id);
            }            
            return View();
        }

        [HttpGet]
        public JsonResult GetElements()
        {
            var courseId = Convert.ToInt32(Request.Form["id"]);
            var elements = _courseItemService.GetElementsByCourseId(courseId);

            return Json(elements);
        }

        [HttpGet]
        public JsonResult GetElementName()
        {
            int courseItemId = Convert.ToInt32(Request.Form["elementId"]);
            var element = _courseItemService.GetCourseItem(courseItemId);
            var typeName = _courseItemService.GetItemType(element.TypeId).Name;

            return Json(element, typeName);
        }

        [HttpPost]
        public JsonResult GetCourseElements()
        {
            var course = Request.Form["course"];
            var result = _courseService.GetCourseElements(Convert.ToInt32(course));
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> SaveCource()
        {
            var form = Request.Form;
            IFormFile? picture = null;
            IFormFile? video = null;
            if (form.Files.Count != 0)
            {
                video = form.Files["video"];
                picture = form.Files["picture"];
            }

            int courseId = Convert.ToInt32(form["courseId"]);
            Course newCourse = new Course();

            newCourse.Name = form["name"].ToString();
            newCourse.AuthorId = Guid.Parse(form["authorId"].ToString());
            newCourse.Checked = false;
            newCourse.Price = Convert.ToDecimal(form["price"].ToString());
            newCourse.CourseSubjectId = Convert.ToInt32(form["subject"]);
            newCourse.LastEdittingDate = DateTime.Now;

            newCourse.PicturePath = await _mediaService.SaveMedia(picture, courseId);
            newCourse.PreviewVideoPath = await _mediaService.SaveMedia(video, courseId);
            
            if (courseId == 0)
            {
                await _courseService.AddCourse(newCourse);
            }
            else
            {
                newCourse.Id = courseId;
                await _courseService.UpdateCourse(newCourse);
            }

            return Json(newCourse.Id);
        }

        [HttpDelete]
        [Route("/Course/DeleteCourseItem/{courseItemId}/{typeId}")]
        public async Task<JsonResult> DeleteCourseItem(int courseItemId, int typeId)
        {
            var item = _courseItemService.GetCourseItem(courseItemId);

            try
            {
                item.StatusId = _context.ItemsStatuses.Where(i => i.Name == "Deleted").FirstOrDefault().Id;
                await _courseItemService.UpdateCourseItem(item);
            }
            catch(Exception ex)
            {
                return Json(ex.Message);
            }

            return Json(true);
        }

        [HttpGet]
        [Route("/Course/Comments/{courseId}/{commentsCount}")]
        public JsonResult GetComments(int courseId, int commentsCount = 10)
        {
            List<CommentDTO> commentsList = new List<CommentDTO>();
            var comments = _context.Comments.Where(c => c.CourseId == courseId);
            foreach (var comment in comments)
            {
                commentsList.Add(new CommentDTO()
                {
                    Id = comment.Id,
                    Text = comment.Text,
                    Rating = comment.Rating,
                    DateAgo = GetTimeSinceDate(comment.DateCreation),
                    UserName = "",              // Add user name Heras O.
                    ProfileImagePath = ""       // Add user profile image name
                });
            }

            commentsList.Add(new CommentDTO()
            {
                Id = 5,
                Text = "Hello",
                Rating = 4,
                DateAgo = "4 month ago",
                UserName = "Heras A.",              // Add user name Heras O.
                ProfileImagePath = ""     // Add user profile image name
            });

            return Json(commentsList);
        }

        [HttpPost]
        public IActionResult LoadPartialPage(string type, string courseItemId)
        {
            if (type == "1")
            {

            }

            int itemId = Convert.ToInt32(courseItemId);
            int courseId = _courseItemService.GetCourseItem(itemId).CourseId;

            Course c = _courseService.GetCourse(courseId);
            Lesson l = _lessonService.GetLessonByCourseItem(itemId);

            LessonDTO lessonPartial = new LessonDTO()
            {
                Id = l.Id,
                Theme = l.Theme,
                Description = l.Description,
                VideoPath = l.VideoPath,
                Body = l.Body,
                DateCreation = l.DateCreation,
                CourseItemId = l.CourseItemId,
                CourseID = c.Id,
                CourseRating = c.Rating
            };

            return PartialView("~/Views/Partial/_LessonPartial.cshtml", lessonPartial);
        }

        public static string GetTimeSinceDate(DateTime date)
        {
            TimeSpan timeSpan = DateTime.Now - date;

            int totalDays = (int)timeSpan.TotalDays;
            int totalWeeks = totalDays / 7;
            int totalMonths = totalDays / 30;
            int totalYears = totalDays / 365;

            if (totalWeeks < 4)
            {
                return $"{totalWeeks} week{(totalWeeks == 1 ? "" : "s")} ago";
            }
            else if (totalMonths < 12)
            {
                return $"{totalMonths} month{(totalMonths == 1 ? "" : "s")} ago";
            }
            else
            {
                return $"{totalYears} year{(totalYears == 1 ? "" : "s")} ago";
            }
        }
    }
}
