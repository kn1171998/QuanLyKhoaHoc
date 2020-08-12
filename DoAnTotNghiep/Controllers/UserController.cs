using AutoMapper;
using DoAnTotNghiep.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebData.Implementation;
using WebData.Models;

namespace DoAnTotNghiep.Controllers
{
    public class UserController : Controller
    {

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;
        private readonly IUserService _IUserServed;
        private readonly ICourseService _courseService;

        public UserController(
            IHostingEnvironment hostingEnvironment,
            IMapper mapper,
            IUserService usersv,
            ICourseService courseService)
        {
            _hostingEnvironment = hostingEnvironment;
            _mapper = mapper;
            _IUserServed = usersv;
            _courseService = courseService;
        }
        [Authorize(Roles = "User")]
        public IActionResult Index(int id)
        {
            var model = _IUserServed.GetById(id);
            var userVM = _mapper.Map<UserVM>(model);
            return View(userVM);
        }
        [Authorize(Roles = "User")]
        public IActionResult Teacher(int id)
        {

            var model = _IUserServed.GetById(id);
            var userVM = _mapper.Map<UserVM>(model);
            ViewBag.Course = _courseService.CountCondition(m => m.Status == true && m.UserId == id);
          //  ViewBag.Student = 
            return View(userVM);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(3145728)]
        public async Task<IActionResult> Update(IFormFile Image, UserVM vm)
        {

            try
            {
                if (Image != null)
                {
                    string nameVideo = Path.GetFileName(Image.FileName);
                    string pathUpload = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\resource\images");
                    string savePath = Path.Combine(pathUpload, nameVideo);
                    string saveDBPath = Path.Combine(@"\resource\images", nameVideo);
                    if (!Directory.Exists(pathUpload))
                    {
                        Directory.CreateDirectory(pathUpload);
                    }
                    using (var stream = new FileStream(savePath, FileMode.CreateNew))
                    {
                        await Image.CopyToAsync(stream);
                    }
                    vm.ImageUrl = saveDBPath;
                }
                var model = _mapper.Map<Users>(vm);
                await _IUserServed.UpdateAsync(model);
                return Json(new { status = true });

            }
            catch (Exception ex)
            {
                return Json(new { status = false });
            }

        }
        [Authorize(Roles = "User")]
        public IActionResult MyCourse()
        {
            var context = _courseService.GetContext();
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = int.Parse(currentUser.FindFirst(ClaimTypes.NameIdentifier).Value);
            MyCourseVM vm = new MyCourseVM();
            vm.listCourse = (from o in context.Orders
                             join od in context.OrderDetails
                             on o.Id equals od.OrderId
                             join u in context.Users
                             on o.UserId equals u.Id
                             join c in context.Courses
                             on od.CourseId equals c.Id
                             where c.Status == true
                                    && o.UserId == currentUserId
                                    && o.Status == OrderStatus.Paid
                             select new CourseVM
                             {
                                 Id = c.Id,
                                 Name = c.Name,
                                 Image = c.Image,
                                 Price = c.Price,
                                 PromotionPrice = c.PromotionPrice,
                                 UserId = c.UserId ?? 0,
                                 FullName = u.FullName
                             }).Distinct();


            return View(vm);
        }
    }
}