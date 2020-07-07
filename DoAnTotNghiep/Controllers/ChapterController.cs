using AutoMapper;
using DoAnTotNghiep.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IWareHouseService _wareHouseService;
        private readonly IHostingEnvironment _hostingEnvironment;
        public readonly IMapper _mapper;

        public ChapterController(IHostingEnvironment hostingEnvironment,
            IChapterService chapterService,
                    IWareHouseService wareHouseService,
                    IMapper mapper,
            ICourseService courseService)
        {
            _hostingEnvironment = hostingEnvironment;
            _chapterService = chapterService;
            _courseService = courseService;
            _wareHouseService = wareHouseService;
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
                        return RedirectToAction("Detail", "Course",new { ID = model.CourseId});
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
    }
}