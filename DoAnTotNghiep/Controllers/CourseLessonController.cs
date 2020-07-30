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
    public class CourseLessonController : Controller
    {
        public readonly ICourseLessonService _courseLessonService;
        private readonly ICourseService _courseService;
        private readonly IWareHouseService _wareHouseService;
        private readonly IChapterService _chapterService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;

        public CourseLessonController(IHostingEnvironment hostingEnvironment,
            ICourseLessonService courseLessonService,
            IWareHouseService wareHouseService,
            IChapterService chapterService,
            IMapper mapper,
            ICourseService courseService)
        {
            _hostingEnvironment = hostingEnvironment;
            _courseLessonService = courseLessonService;
            _courseService = courseService;
            _chapterService = chapterService;
            _mapper = mapper;
            _wareHouseService = wareHouseService;
        }

        public IActionResult Index(int ID)
        {
            var vm = new CourseVM();
            if (ID != 0)
            {
                vm.Id = ID;
            }
            return View(vm);
        }
        public IActionResult _Index(int ID)
        {
            Dictionary<int, IEnumerable<CourseLessons>> lessonVMs = new Dictionary<int, IEnumerable<CourseLessons>>();
            var lstChapter = _chapterService.GetCondition(m => m.CourseId == ID)
                .Select(m => new
                {
                    m.Id,
                    m.NameChapter,
                    m.CourseId,
                    m.OrderChapter
                })
                .OrderBy(m => m.OrderChapter);
            foreach (var item in lstChapter)
            {
                var lstLesson = _courseLessonService.GetCondition(m => m.ChapterId == item.Id)
                    .Select(m => new CourseLessons
                    {
                        Id = m.Id,
                        Name = m.Name,
                        ChapterId = m.ChapterId,
                        SortOrder = m.SortOrder
                    })
                    .OrderBy(m => m.SortOrder);
                lessonVMs.Add(item.Id, lstLesson);
            }

            return Json(new { status = true, lstChapter = lstChapter, lessonVMs = lessonVMs });
        }
        public bool CheckFile(string path)
        {
            return System.IO.File.Exists(path);
        }
        public IActionResult Create(int ID, int ChapterId)
        {
            var vm = new CourseLessonVM();
            if (ID != 0)
            {
                var model = _courseLessonService.GetById(ID);
                if (model == null)
                    return NotFound();
                vm = _mapper.Map<CourseLessonVM>(model);
                if (vm.VideoPath != null)
                {
                    var fileVideo = Path.Combine(Directory.GetCurrentDirectory(), vm.VideoPath);
                    if (!CheckFile(fileVideo))
                        vm.VideoDisplay = Path.GetFileName(fileVideo);
                }
                if (vm.SlidePath != null)
                {
                    var fileSlide = Path.Combine(Directory.GetCurrentDirectory(), vm.SlidePath);
                    if (!CheckFile(fileSlide))
                        vm.SlideDisplay = Path.GetFileName(fileSlide);
                }
            }
            else
            {
                var numLesson = _courseLessonService.CountCondition(m => m.ChapterId == ChapterId);
                vm.ChapterId = ChapterId;
                vm.SortOrder = ++numLesson;
            }
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(int.MaxValue)]
        public async Task<IActionResult> Create(CourseLessonVM vm)
        {
            if (ModelState.IsValid)
            {
                string pathVideo = vm.VideoPath;
                string pathSlide = vm.SlidePath;
                try
                {
                    var idCourse = _chapterService.GetCondition(m => m.Id == vm.ChapterId).Select(m => m.CourseId).FirstOrDefault();
                    if (vm.Video != null)
                    {
                        string nameVideo = Path.GetFileName(vm.Video.FileName);
                        string pathUpload = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\resource\videos\" + idCourse);
                        if (CheckFile(Path.Combine(pathUpload, nameVideo)))
                        {
                            string nameTempVideo = Path.GetFileNameWithoutExtension(nameVideo);
                            nameVideo = nameTempVideo + DateTime.Now.ToString("yyyyMMddhhmmssfff") + Path.GetExtension(nameVideo);
                        }
                        string savePath = Path.Combine(pathUpload, nameVideo);
                        string saveDBPath = Path.Combine(@"\resource\videos\" + idCourse, nameVideo);
                        if (!Directory.Exists(pathUpload))
                        {
                            Directory.CreateDirectory(pathUpload);
                        }
                        using (var stream = new FileStream(savePath, FileMode.CreateNew))
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
                        return Json(new { status = true });
                    }
                    else
                    {
                        vm.Status = false;
                        string name = Directory.GetCurrentDirectory();
                        if (!string.IsNullOrEmpty(pathVideo) && !string.IsNullOrEmpty(vm.VideoPath))
                        {
                            string urlVideo = name + @"\wwwroot\" + pathVideo;
                            if (CheckFile(urlVideo))
                            {
                                System.IO.File.Delete(urlVideo);
                            }
                        }
                        if (!string.IsNullOrEmpty(pathSlide) && !string.IsNullOrEmpty(vm.SlidePath))
                        {
                            string urlSlide = name + @"\wwwroot\" + pathSlide;
                            if (CheckFile(urlSlide))
                            {
                                System.IO.File.Delete(urlSlide);
                            }
                        }
                        var model = _mapper.Map<CourseLessons>(vm);
                        await _courseLessonService.UpdateAsync(model);
                        return Json(new { status = true });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { status = false });
                }
            }
            return Json(new { status = false });
        }
        [HttpPost]
        [RequestSizeLimit(int.MaxValue)]
        public async Task<IActionResult> UploadVideo(List<IFormFile> VideoPath)
        {
            try
            {
                if (VideoPath != null)
                {
                    string nameVideo = Path.GetFileName(VideoPath[0].FileName);
                    string nameFolderCourse = "Test";
                    string pathUpload = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\resource\videos", nameFolderCourse);
                    string savePath = Path.Combine(pathUpload, nameVideo);
                    string saveDBPath = Path.Combine(@"\resource\videos", nameFolderCourse, nameVideo);
                    var a = Directory.Exists(Path.GetDirectoryName(pathUpload));
                    if (!Directory.Exists(pathUpload))
                    {
                        Directory.CreateDirectory(pathUpload);
                    }
                    using (var stream = new FileStream(savePath, FileMode.Create))
                    {
                        await VideoPath[0].CopyToAsync(stream);
                    }
                    WareHouse wareHouse = new WareHouse();
                    wareHouse.NameFolder = nameFolderCourse;
                    wareHouse.NamePath = saveDBPath;
                    wareHouse.NameFile = nameVideo;
                    await _wareHouseService.CreateAsync(wareHouse);
                    return Json(new { result = true, namevideo = nameVideo, pathvideo = saveDBPath });
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = false });
            }
            return Json(new { result = false });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int ID)
        {
            if (ID == 0)
            {
                return NotFound();
            }
            try
            {
                string name1 = Directory.GetCurrentDirectory();
                quanlykhoahocContext context = _courseLessonService.GetContext();
                using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var model = _courseLessonService.GetById(ID);
                        var softOrderCurrent = model.SortOrder;
                        var idCourse = _chapterService.GetById(model.ChapterId??0);
                        var listUpdateOrderSort = _courseLessonService.GetCondition(m => m.SortOrder > model.SortOrder && m.ChapterId == model.ChapterId).OrderBy(m=>m.SortOrder).Select(m => m).ToList();
                        await _courseLessonService.Delete(ID);
                        int i = 0;
                        foreach (var item in listUpdateOrderSort)
                        {
                            var modelUpdateOrderSoft = _courseLessonService.GetById(item.Id);
                            if (i == 0)
                                modelUpdateOrderSoft.SortOrder = softOrderCurrent;

                            else
                                modelUpdateOrderSoft.SortOrder = ++softOrderCurrent;
                            i++;
                            await _courseLessonService.UpdateAsync(modelUpdateOrderSoft);
                        }
                        if (!string.IsNullOrEmpty(model.VideoPath))
                        {
                            string urlVideo = name1 + @"\wwwroot\" + model.VideoPath;
                            if (CheckFile(urlVideo))
                            {
                                System.IO.File.Delete(urlVideo);
                            }
                        }
                        if (!string.IsNullOrEmpty(model.SlidePath))
                        {
                            string urlSlide = name1 + @"\wwwroot\" + model.SlidePath;
                            if (CheckFile(urlSlide))
                            {
                                System.IO.File.Delete(urlSlide);
                            }
                        }
                        transaction.Commit();
                        return RedirectToAction("Detail", "Course", new { ID = idCourse });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Error occurred.");
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}