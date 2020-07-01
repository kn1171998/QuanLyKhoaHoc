using AutoMapper;
using DoAnTotNghiep.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebData.Implementation;
using WebData.Models;

namespace DoAnTotNghiep.Controllers
{
    public class CourseController : Controller
    {
        public readonly ICourseLessonService _courseLessonService;
        private readonly ICourseService _courseService;
        private readonly ICourseCategoryService _courseCategoryService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;
        private readonly IChapterService _chapterService;

        public CourseController(ICourseService courseService,
            ICourseLessonService courseLessonService,
            IHostingEnvironment hostingEnvironment,
            ICourseCategoryService courseCategoryService,
            IChapterService chapterService,
            IMapper mapper)
        {
            _hostingEnvironment = hostingEnvironment;
            _courseService = courseService;
            _courseLessonService = courseLessonService;
            _chapterService = chapterService;
            _courseCategoryService = courseCategoryService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var courses = new CourseVM();
            ViewBag.IsSuccess = TempData["IsSuccess"];
            return View(courses);
        }

        public IActionResult _Index(string searchCourse, int page, int pageSize = 3)
        {
            var model = new Object();
            int totalRow = 0;
            if (string.IsNullOrEmpty(searchCourse))
            {
                model = _courseService.GetPaging(null, out totalRow, page, pageSize, x => x.DateCreated).
                    Select(m => new
                    {
                        m.Id,
                        m.Name,
                        m.Image,
                        m.Price,
                        m.PromotionPrice,
                        m.Status,
                        m.CategoryId
                    }).ToList();
            }
            else
            {
                model = _courseService.GetPaging(m => m.Name.Contains(searchCourse), out totalRow, page, pageSize, x => x.DateCreated).
                    Select(m => new
                    {
                        m.Id,
                        m.Name,
                        m.Image,
                        m.Price,
                        m.PromotionPrice,
                        m.Status,
                        m.CategoryId
                    }).ToList(); ;
            }
            return Json(new
            {
                data = model,
                total = totalRow,
                status = totalRow == 0 ? false : true
            });
        }

        public IActionResult Create(int ID)
        {
            var vm = new CourseVM();
            var lstParent = _courseCategoryService.GetParentCategory();
            if (ID != 0)
            {
                var modelcourse = _courseService.GetById(ID);
                if (modelcourse == null)
                {
                    return NotFound();
                }
                vm = _mapper.Map<CourseVM>(modelcourse);
                var child = _courseCategoryService.GetById(vm.CategoryId);
                var selectValueParent = _courseCategoryService.GetById(int.Parse(child.ParentId.ToString()));
                var lstChild = _courseCategoryService.GetChildCategory(child.ParentId);
                vm.lstCategories = new SelectList(lstParent, "Id", "Name", selectValueParent);
                vm.lstChildCategories = new SelectList(lstChild, "Id", "Name", child);
            }
            else
            {
                vm.lstCategories = new SelectList(lstParent, "Id", "Name");
                vm.lstChildCategories = null;
            }
            return View(vm);
        }


        

        public IActionResult GetChildCategories(int? parentId)
        {
            if (parentId == null)
                return NotFound();
            var child = _courseCategoryService.GetChildCategory(parentId).ToList();
            return Json(new
            {
                child = child,
                status = child.Count() == 0 ? false : true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(3145728)]
        public async Task<IActionResult> Create(List<IFormFile> LImage, CourseVM vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (LImage != null && LImage.Count > 0)
                    {
                        string nameVideo = Path.GetFileName(LImage[0].FileName);
                        string pathUpload = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\resource\images");
                        string savePath = Path.Combine(pathUpload, nameVideo);
                        string saveDBPath = Path.Combine(@"\resource\images", nameVideo);
                        if (!Directory.Exists(pathUpload))
                        {
                            Directory.CreateDirectory(pathUpload);
                        }
                        using (var stream = new FileStream(savePath, FileMode.Create))
                        {
                            await LImage[0].CopyToAsync(stream);
                        }
                        vm.Image = saveDBPath;
                    }
                    vm.UserId = 1;
                    if (vm.Id == 0)
                    {
                        vm.DateCreated = DateTime.Now;
                        var model = _mapper.Map<Courses>(vm);
                        await _courseService.CreateAsync(model);
                        TempData["IsSuccess"] = true;
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        var model = _mapper.Map<Courses>(vm);
                        await _courseService.UpdateAsync(model);
                        TempData["IsSuccess"] = true;
                        TempData.Keep("IsSuccess");
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    TempData["IsSuccess"] = false;
                    return RedirectToAction(nameof(Index));
                }
            }
            TempData["IsSuccess"] = false;
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Update(int ID)
        {
            var modelcourse = _courseService.GetById(ID);
            if (modelcourse == null)
            {
                return NotFound();
            }
            var vm = _mapper.Map<CourseVM>(modelcourse);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CourseVM vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var model = _mapper.Map<Courses>(vm);
                    await _courseService.UpdateAsync(model);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    return View(vm);
                }
            }
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            bool result = true;
            try
            {
                await _courseService.Delete(id);
                return Json(result);
            }
            catch (Exception)
            {
                result = false;
                return Json(result);
            }
        }
        public IActionResult Detail(int ID)
        {
            var vm = new CourseVM();
            if (ID != 0)
            {
                vm.Id = ID;
            }
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(int.MaxValue)]
        public async Task<IActionResult> Detail(CourseLessonVM vm)
        {
            var idCourse = _chapterService.GetCondition(m => m.Id == vm.ChapterId).Select(m => m.CourseId).FirstOrDefault();
            if (ModelState.IsValid)
            {
                try
                {                    
                    if (vm.Video != null)
                    {

                        string nameVideo = Path.GetFileName(vm.Video.FileName);
                        string pathUpload = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\resource\videos\" + idCourse);
                        string savePath = Path.Combine(pathUpload, nameVideo);
                        string saveDBPath = Path.Combine(@"\resource\videos\" + idCourse, nameVideo);
                        if (!Directory.Exists(pathUpload))
                        {
                            Directory.CreateDirectory(pathUpload);
                        }
                        using (var stream = new FileStream(savePath, FileMode.Create))
                        {
                            await vm.Video.CopyToAsync(stream);
                        }
                        vm.VideoPath = saveDBPath;
                    }
                    if (vm.Slide != null)
                    {
                        string nameVideo = Path.GetFileName(vm.Slide.FileName);
                        string pathUpload = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\resource\slides\" + idCourse);
                        string savePath = Path.Combine(pathUpload, nameVideo);
                        string saveDBPath = Path.Combine(@"\resource\slides\" + idCourse, nameVideo);
                        if (!Directory.Exists(pathUpload))
                        {
                            Directory.CreateDirectory(pathUpload);
                        }
                        using (var stream = new FileStream(savePath, FileMode.Create))
                        {
                            await vm.Slide.CopyToAsync(stream);
                        }
                        vm.SlidePath = saveDBPath;
                    }
                    if (vm.Id == 0)
                    {
                        vm.Status = false;
                        var model = _mapper.Map<CourseLessons>(vm);
                        await _courseLessonService.CreateAsync(model);
                        return RedirectToAction("Detail",new { ID= idCourse });
                    }
                    else
                    {
                        vm.Status = false;
                        var model = _mapper.Map<CourseLessons>(vm);
                        await _courseLessonService.UpdateAsync(model);
                        return RedirectToAction("Detail", new { ID = idCourse });
                    }
                }
                catch (Exception)
                {
                    return RedirectToAction("Detail", new { ID = idCourse });
                }
            }

            return RedirectToAction("Detail", new { ID = idCourse });
        }
    }
}