using AutoMapper;
using DoAnTotNghiep.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebData.Implementation;
using WebData.Models;

namespace DoAnTotNghiep.Controllers
{
    public class ChapterController : Controller
    {
        public readonly IChapterService _chapterService;
        private readonly ICourseService _courseService;
        private readonly ICourseLessonService _courseLessonService;
        private readonly IWareHouseService _wareHouseService;
        private readonly IHostingEnvironment _hostingEnvironment;
        public readonly IMapper _mapper;

        public ChapterController(IHostingEnvironment hostingEnvironment,
            IChapterService chapterService,
                    IWareHouseService wareHouseService,
                    ICourseLessonService courseLessonService,
                    IMapper mapper,
            ICourseService courseService)
        {
            _hostingEnvironment = hostingEnvironment;
            _chapterService = chapterService;
            _courseService = courseService;
            _wareHouseService = wareHouseService;
            _courseLessonService = courseLessonService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create(int ID, int CourseId)
        {
            var vm = new ChapterVM();
            if (ID != 0)
            {
                var model = _chapterService.GetById(ID);
                if (model == null)
                    return NotFound();
                vm = _mapper.Map<ChapterVM>(model);
            }
            else
            {
                var numChapter = _chapterService.CountCondition(m => m.CourseId == CourseId);
                vm.OrderChapter = ++numChapter;
                vm.CourseId = CourseId;
            }
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ChapterVM vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (vm.Id == 0)
                    {
                        var model = _mapper.Map<Chapter>(vm);
                        await _chapterService.CreateAsync(model);
                        return RedirectToAction("Detail", "Course", new { ID = model.CourseId });
                    }
                    else
                    {
                        var model = _mapper.Map<Chapter>(vm);
                        await _chapterService.UpdateAsync(model);
                        return RedirectToAction("Detail", "Course", new { ID = model.CourseId });
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Detail", "Course", new { ID = vm.CourseId });
                }
            }
            return RedirectToAction("Detail", "Course", new { ID = vm.CourseId });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int ID)
        {
            if (ID == 0)
            {
                return NotFound();
            }
            quanlykhoahocContext context = _courseService.GetContext();
            using (IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var listlesson = _courseLessonService.GetCondition(m => m.ChapterId == ID);
                    foreach (var item in listlesson)
                    {
                        await _courseLessonService.Delete(item);
                    }
                    await _chapterService.Delete(ID);
                    transaction.Commit();
                    var idCourse = _chapterService.GetById(ID);
                    return RedirectToAction("Detail", "Course", new { ID = idCourse });
                }
                catch
                {
                    transaction.Rollback();
                    Console.WriteLine("Error occurred.");
                    return NotFound();
                }
            }
        }
    }
}