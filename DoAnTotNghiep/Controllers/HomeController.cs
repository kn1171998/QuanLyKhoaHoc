using AutoMapper;
using DoAnTotNghiep.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebData.Implementation;
using WebData.Models;

namespace DoAnTotNghiep.Controllers
{
    public class HomeController : Controller
    {
        public readonly IUserService _userService;
        public readonly IMapper _mapper;
        public readonly ICourseCategoryService _courseCategoryService;
        public readonly ICourseService _courseService;
        public readonly IChapterService _chapterService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(IUserService userService
            , IMapper mapper
            , ICourseCategoryService courseCategoryService
            , ICourseService courseService
            , IChapterService chapterService
            , IHostingEnvironment hostingEnvironment)
        {
            _userService = userService;
            _mapper = mapper;
            _courseCategoryService = courseCategoryService;
            _courseService = courseService;
            _chapterService = chapterService;
            _hostingEnvironment = hostingEnvironment;
        }

        #region Cookie

        public string GetCookie(string key)
        {
            return HttpContext.Request.Cookies[key];
        }

        public void SetCookie(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();
            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);
            HttpContext.Response.Cookies.Append(key, value, option);
        }

        public void RemoveCookie(string key)
        {
            HttpContext.Response.Cookies.Delete(key);
        }

        #endregion Cookie

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public class FacebookAccount
        {
            public string id { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string age_range { get; set; }
            public string birthday { get; set; }
            public string gender { get; set; }
            public string locale { get; set; }
            public Picture picture { get; set; }
        }

        public class Picture
        {
            public Data data { get; set; }
        }

        public class Data
        {
            public int height { get; set; }
            public bool is_silhouette { get; set; }
            public string url { get; set; }
            public int width { get; set; }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FacebookLogin(string Token)
        {
            if (string.IsNullOrEmpty(Token))
            {
                return View();
            }
            var httpClient = new HttpClient { BaseAddress = new Uri("https://graph.facebook.com/v2.9/") };
            var response = await httpClient.GetAsync($"me?access_token={Token}&fields=id,name,email,first_name,last_name,age_range,birthday,gender,locale,picture");
            if (!response.IsSuccessStatusCode) return BadRequest();
            var result = await response.Content.ReadAsStringAsync();
            var facebookAccount = JsonConvert.DeserializeObject<FacebookAccount>(result);
            var facebookUser = _userService.GetCondition(m => m.FacebookId.ToString() == facebookAccount.id).FirstOrDefault();
            if (facebookUser == null)
            {
                try
                {
                    var user = new Users { TypeUser = TypeUser.User, FullName = facebookAccount.name, Email = facebookAccount.email, FacebookId = int.Parse(facebookAccount.id) };
                    await _userService.CreateAsync(user);
                    var getIdUs = _userService.GetCondition(m => m.FacebookId.ToString() == facebookAccount.id).FirstOrDefault();
                    if (getIdUs != null)
                    {
                        string cookie = GetCookie(getIdUs.Id.ToString());
                        if (string.IsNullOrEmpty(cookie))
                        {
                            SetCookie(getIdUs.Id.ToString(), getIdUs.Id.ToString(), DefineCommon.ExpireCookie);
                        }
                    }
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
            else
            {
                string cookie = GetCookie(facebookUser.Id.ToString());
                if (string.IsNullOrEmpty(cookie))
                {
                    SetCookie(facebookUser.Id.ToString(), facebookUser.Id.ToString(), DefineCommon.ExpireCookie);
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(UserVM vm)
        {
            if (ModelState.IsValid)
            {
                var checkUserExist = _userService.CountCondition(m => m.Email == vm.Email);
                if (checkUserExist > 0)
                {
                    ModelState.AddModelError("Email", "Email đã tồn tại! Vui lòng chọn email khác");
                    return View(vm);
                }
                try
                {
                    var model = _mapper.Map<Users>(vm);
                    await _userService.CreateAsync(model);
                    return Redirect("Index");
                }
                catch (Exception ex)
                {
                    return View(vm);
                }
            }
            return View(vm);
        }
        public IActionResult ListCategory()
        {
            var category = _courseCategoryService.GetCondition(m => m.Status == true && m.ParentId == 0).Select(m => new
            {
                m.Id,
                m.Name,
                m.SortOrder
            }).OrderBy(m => m.SortOrder);
            Dictionary<int, List<CourseCategories>> listCategoryChild = new Dictionary<int, List<CourseCategories>>();
            foreach (var item in category)
            {
                var categoryChild = _courseCategoryService.GetChildCategory(item.Id).Select(m => new CourseCategories
                {
                    Id = m.Id,
                    Name = m.Name,
                    ParentId = m.ParentId,
                    SortOrder = m.SortOrder
                }).OrderBy(m => m.SortOrder).ToList();
                if (categoryChild.Count > 0)
                {
                    listCategoryChild.Add(item.Id, categoryChild);
                }
                else
                {
                    listCategoryChild.Add(item.Id, new List<CourseCategories>());
                }
            }
            bool status = false;
            if (listCategoryChild.Count > 0)
            {
                status = true;
            }
            return Json(new { status = status, parentCategory = category, listChild = listCategoryChild });
        }
        public IActionResult ListAllCourseTop(int ID)
        {
            var categoryChild = _courseCategoryService.GetCondition(m => m.ParentId == ID && m.Status == true).Select(m => new
            {
                m.Id,
                m.Name,
                m.SortOrder
            }).OrderBy(m => m.SortOrder).ToList();
            List<object> course = new List<object>();
            foreach (var item in categoryChild)
            {
                var listCourse = _courseService.GetCondition(m => m.CategoryId == item.Id)
                    .Select(m => new
                    {
                        m.Id,
                        m.UserId,
                        m.Name,
                        m.Image,
                        m.PromotionPrice,
                        m.Price,
                        parentId = ID
                    }).ToList();
                if (listCourse.Count() > 0)
                    course.AddRange(listCourse);
            }
            return Json(new { status = true, topCourse = course });
        }
        public IActionResult Detail(int ID)
        {
            DetailHomeVM vm = new DetailHomeVM();
            var context = _courseService.GetContext();
            var model = from c in context.Courses
                        join u in context.Users
                        on c.UserId equals u.Id
                        join cate in context.CourseCategories
                        on c.CategoryId equals cate.Id
                        where c.Id == ID
                        select new DetailHomeVM
                        {
                            IdCourse = c.Id,
                            lstChapter = (context.Chapter.Where(m => m.CourseId == ID).ToList()),
                            lstCourseLesson = (context.Chapter
                                                      .Join(context.CourseLessons,
                                                            chap => chap.Id,
                                                            cl => cl.ChapterId,
                                                            (chap, cl) => new
                                                            {
                                                                Chapter = chap,
                                                                CourseLessons = cl
                                                            })
                                                       .Where(m => m.Chapter.CourseId == ID)
                                                       .Select(m => m.CourseLessons).ToList()),
                            Name = c.Name,
                            NameCategory = cate.Name,
                            FullName = u.FullName,
                            UserId = u.Id,
                            PromotionPrice = c.PromotionPrice ?? 0,
                            Price = c.Price ?? 0,
                            Content = c.Content,
                            Image = c.Image,
                            Description = c.Description
                        };
            vm = model.FirstOrDefault();
            return View(vm);
        }
        public IActionResult View(int ID)
        {
            DetailHomeVM vm = new DetailHomeVM();
            var context = _courseService.GetContext();
            var model = from c in context.Courses
                        join u in context.Users
                        on c.UserId equals u.Id
                        join cate in context.CourseCategories
                        on c.CategoryId equals cate.Id
                        where c.Id == ID
                        select new DetailHomeVM
                        {
                            IdCourse = c.Id,
                            lstChapter = (context.Chapter.Where(m => m.CourseId == ID).ToList()),
                            lstCourseLesson = (context.Chapter
                                                      .Join(context.CourseLessons,
                                                            chap => chap.Id,
                                                            cl => cl.ChapterId,
                                                            (chap, cl) => new
                                                            {
                                                                Chapter = chap,
                                                                CourseLessons = cl
                                                            })
                                                       .Where(m => m.Chapter.CourseId == ID)
                                                       .Select(m => m.CourseLessons).ToList()),
                            Name = c.Name,
                            NameCategory = cate.Name,
                            FullName = u.FullName,
                            UserId = u.Id,
                            PromotionPrice = c.PromotionPrice ?? 0,
                            Price = c.Price ?? 0,
                            Content = c.Content,
                            Image = c.Image,
                            Description = c.Description
                        };
            vm = model.FirstOrDefault();
            return View(vm);
        }
        public ActionResult GetMedia(string path)
        {
            string fn = _hostingEnvironment.WebRootPath + path;
            var memoryStream = new MemoryStream(System.IO.File.ReadAllBytes(fn));
            return new FileStreamResult(memoryStream, MimeMapping.MimeUtility.GetMimeMapping(Path.GetFileName(fn)));
        }
    }
}