using AutoMapper;
using DoAnTotNghiep.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebData.Implementation;
using WebData.Models;

namespace DoAnTotNghiep.Controllers
{
    public class LessonCommentController : Controller
    {
        private readonly ILessonCommentService _lessonCommentService;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _hostingEnvironment;
        private quanlykhoahocContext _context;
        public LessonCommentController(ILessonCommentService lessonCommentService,
                  IHostingEnvironment hostingEnvironment,
            IMapper mapper
            )
        {
            _hostingEnvironment = hostingEnvironment;
            _lessonCommentService = lessonCommentService;
            _mapper = mapper;
            _context = _lessonCommentService.GetContext();
        }

        [HttpGet]
        public IActionResult LoadComment(int idlesson)
        {
            var model = from u in _context.Users
                        join lc in _context.LessonComments
                        on u.Id equals lc.UserId
                        where lc.LessonId == idlesson
                        select new
                        {
                            lc.LessonId,
                            u.FullName,
                            u.ImageUrl,
                            lc.Content
                        };
            return Json(new
            {
                data = model,
                status = model.Count() > 0 ? true : false
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(string content, int idlesson)
        {

            try
            {
                if (!string.IsNullOrEmpty(content) && idlesson != 0)
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        LessonComments lessonComments = new LessonComments();
                        ClaimsPrincipal currentUser = this.User;
                        var currentUserName = int.Parse(currentUser.FindFirst(ClaimTypes.NameIdentifier).Value);
                        lessonComments.UserId = currentUserName;
                        lessonComments.LessonId = idlesson;
                        lessonComments.Content = content;
                        await _lessonCommentService.CreateAsync(lessonComments);
                        return Json(new { status = true, idlesson = idlesson });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false });
            }

            return Json(new { status = false });
        }


        //public async Task<IActionResult> Delete(int ID)
        //{
        //    bool result = true;
        //    try
        //    {
        //        await _iUserService.Delete(ID);
        //        return Json(result);
        //    }
        //    catch (Exception)
        //    {
        //        result = false;
        //        return Json(result);
        //    }
        //}

    }
}