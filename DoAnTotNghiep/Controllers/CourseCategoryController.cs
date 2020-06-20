using AutoMapper;
using DoAnTotNghiep.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
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

        public CourseCategoryController(ICourseCategoryService courseCategoryService,
            IHostingEnvironment hostingEnvironment,
            IMapper mapper)
        {
            _hostingEnvironment = hostingEnvironment;
            _courseCategoryService = courseCategoryService;
            _mapper = mapper;
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
        [HttpPost]
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
    }
}