using AutoMapper;
using DoAnTotNghiep.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebData.Implementation;
using WebData.Models;

namespace DoAnTotNghiep.Controllers
{
    public class CourseCategoryController : Controller
    {
        private readonly ICourseCategoryService _courseCategoryService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;
        private readonly ICourseService _courseService;

        public CourseCategoryController(ICourseCategoryService courseCategoryService,
            IHostingEnvironment hostingEnvironment,
            IMapper mapper,
            ICourseService courseService
            )
        {
            _hostingEnvironment = hostingEnvironment;
            _courseCategoryService = courseCategoryService;
            _mapper = mapper;
            _courseService = courseService;
        }

        public IActionResult Index()
        {
            var courses = new CourseCategoryVM();
            return View(courses);
        }

        public IActionResult _Index(string searchName, int page, int pageSize = 3)
        {
            var model = new Object();
            int totalRow = 0;
            if (string.IsNullOrEmpty(searchName))
            {
                model = _courseCategoryService.GetPaging(null, out totalRow, page, pageSize, x => x.Id).
                 Select(m => new
                 {
                     m.Id,
                     m.Name,
                     m.SortOrder,
                     m.Status
                 }).ToList();
            }
            else
            {
                model = _courseCategoryService.GetPaging(m => m.Name.Contains(searchName), out totalRow, page, pageSize, x => x.Id).
                  Select(m => new
                  {
                      m.Id,
                      m.Name,
                      m.SortOrder,
                      m.Status
                  }).ToList();
            }
            return Json(new
            {
                data = model,
                total = totalRow,
                status = true
            });
        }

        public IActionResult Create(int ID)
        {
            var vm = new CourseCategoryVM();
            if (ID == 0)
            {
                vm.listCourseCategory = _courseCategoryService.GetAll();
            }
            else
            {
                var modelcourse = _courseCategoryService.GetById(ID);
                if (modelcourse == null)
                {
                    return NotFound();
                }
                vm = _mapper.Map<CourseCategoryVM>(modelcourse);
                vm.listCourseCategory = _courseCategoryService.GetAll();
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseCategoryVM vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Create
                    if (vm.ID == 0)
                    {
                        var model = _mapper.Map<CourseCategories>(vm);
                        await _courseCategoryService.CreateAsync(model);
                        ViewBag.IsSuccess = true;
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        var model = _mapper.Map<CourseCategories>(vm);
                        await _courseCategoryService.UpdateAsync(model);
                        ViewBag.IsSuccess = true;
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.IsSuccess = false;
                    return View(vm);
                }
            }
            return View(vm);
        }
        public IActionResult LoadCourse(string id)
        {
            var courseCategoryService = _courseCategoryService.GetParentCategory();
            var countC = courseCategoryService.Count();
            return Json(new
            {
                data = courseCategoryService,
                status = countC == 0 ? false : true
            });
        }

        public async Task<IActionResult> Delete(int ID)
        {
            bool result = true;
            try
            {
                await _courseCategoryService.Delete(ID);
                return Json(result);
            }
            catch (Exception)
            {
                result = false;
                return Json(result);
            }
        }

        public IActionResult CategoryToCourse(int id)
        {
            var context = _courseService.GetContext();
            var d = from c in context.Courses
                    join u in context.Users
                    on c.UserId equals u.Id
                    join cate in context.CourseCategories
                    on c.CategoryId equals cate.Id
                    where cate.Id == id
                    select new CourseVM
                    {
                        Id = c.Id,
                        Name = c.Name,
                        FullName = u.FullName,
                        UserId = u.Id,
                        PromotionPrice = c.PromotionPrice ?? 0,
                        Price = c.Price,
                        Content = c.Content,
                        Image = c.Image,
                        Description = c.Description
                    };
            var courseVM = new CourseVM();
            courseVM.lstCourse = d.ToList();
            courseVM.CategoryId = id;
            return View("CategoryToCourse", courseVM);
        }
        public IActionResult search_home(string search)
        {
            var context = _courseService.GetContext();
            var d = from c in context.Courses
                    join u in context.Users
                    on c.UserId equals u.Id
                    join cate in context.CourseCategories
                    on c.CategoryId equals cate.Id
                    where c.Name.Contains(search)
                    select new CourseVM
                    {
                        Id = c.Id,
                        Name = c.Name,
                        FullName = u.FullName,
                        UserId = u.Id,
                        PromotionPrice = c.PromotionPrice ?? 0,
                        Price = c.Price,
                        Content = c.Content,
                        Image = c.Image,
                        Description = c.Description,
                        CategoryId = c.CategoryId??0
                    };
            var courseVM = new CourseVM();
            courseVM.lstCourse = d.ToList();
            courseVM.CategoryId = courseVM.lstCourse[0].CategoryId;
            return View("CategoryToCourse", courseVM);
        }
        public IActionResult GetChildCategories(int id)
        {
            if (id == 0)
                return Json(new
                {
                    child = "",
                    status = false
                });

            var model = _courseCategoryService.GetById(id);
            var child = _courseCategoryService.GetChildCategory(model.ParentId).ToList();
            return Json(new
            {
                child = child,
                status = child.Count() == 0 ? false : true
            });
        }

        public IActionResult Sort(int id, string sortPrice, string radiofree, string search)
        {
            var context = _courseService.GetContext();
            List<CourseVM> d = new List<CourseVM>();
            if (string.IsNullOrEmpty(sortPrice))
            {
                d = (from c in context.Courses
                     join u in context.Users
                     on c.UserId equals u.Id
                     join cate in context.CourseCategories
                     on c.CategoryId equals cate.Id
                     where cate.Id == id
                     select new CourseVM
                     {
                         Id = c.Id,
                         Name = c.Name,
                         FullName = u.FullName,
                         UserId = u.Id,
                         PromotionPrice = c.PromotionPrice ?? 0,
                         Price = c.Price,
                         Content = c.Content,
                         Image = c.Image,
                         Description = c.Description,
                         IsFree = c.IsFree ?? false
                     }).ToList();
            }
            else
            {
                d = sortPrice == "1" ? (from c in context.Courses
                                        join u in context.Users
                                        on c.UserId equals u.Id
                                        join cate in context.CourseCategories
                                        on c.CategoryId equals cate.Id
                                        where cate.Id == id
                                        select new CourseVM
                                        {
                                            Id = c.Id,
                                            Name = c.Name,
                                            FullName = u.FullName,
                                            UserId = u.Id,
                                            PromotionPrice = c.PromotionPrice ?? 0,
                                            Price = c.Price,
                                            Content = c.Content,
                                            Image = c.Image,
                                            Description = c.Description,
                                            IsFree = c.IsFree ?? false
                                        }).OrderByDescending(m => m.Price).ToList() :
                    (from c in context.Courses
                     join u in context.Users
                     on c.UserId equals u.Id
                     join cate in context.CourseCategories
                     on c.CategoryId equals cate.Id
                     where cate.Id == id
                     select new CourseVM
                     {
                         Id = c.Id,
                         Name = c.Name,
                         FullName = u.FullName,
                         UserId = u.Id,
                         PromotionPrice = c.PromotionPrice ?? 0,
                         Price = c.Price,
                         Content = c.Content,
                         Image = c.Image,
                         Description = c.Description,
                         IsFree = c.IsFree ?? false
                     }).OrderBy(m => m.Price).ToList();

            }

            var listTemp = new List<CourseVM>();
            var courseVM = new CourseVM();
            if (radiofree == "0") //free là 0
            {
                listTemp = d.Where(m => m.IsFree == true).Select(m => m).ToList();
                courseVM.IsFree = true;
            }
            else if (radiofree == "1")
            {
                listTemp = d.Where(m => m.IsFree == false).Select(m => m).ToList();
                courseVM.IsFree = false;
            }
            else
            {
                listTemp = d.ToList();
            }
            if (string.IsNullOrEmpty(search))
                search = "";
            listTemp = listTemp.Where(m => m.Name.Contains(search)).ToList();
            courseVM.lstCourse = listTemp;
            courseVM.CategoryId = id;

            return Json(new { status = true, data = listTemp });
        }
    }
}