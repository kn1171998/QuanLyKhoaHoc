using AutoMapper;
using DoAnTotNghiep.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
    public class DiscountController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IDiscountService _discountService;
        private readonly IDiscountCourseService _discountCourseService;
        private readonly ICourseCategoryService _courseCategoryService;
        private readonly IMapper _mapper;

        public DiscountController(ICourseService courseService,
            IDiscountService discountService,
            ICourseCategoryService courseCategoryService,
            IDiscountCourseService discountCourseService,
            IMapper mapper)
        {
            _courseService = courseService;
            _courseCategoryService = courseCategoryService;
            _discountService = discountService;
            _discountCourseService = discountCourseService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var discount = new DiscountVM();
            return View(discount);
        }
        public async Task<IActionResult> LoadDiscountHasCourse(int id)
        {
            var _context = _discountService.GetContext();
            DiscountDetailVM vm = new DiscountDetailVM();
            var discountHasCourse = await (from d in _context.Discount
                                           join dc in _context.DiscountCourse
                                           on d.Id equals dc.Iddiscount
                                           join c in _context.Courses
                                           on dc.Idcourse equals c.Id
                                           where d.Id == id
                                           select new DiscountDetailVM
                                           {
                                               CodeDiscount = d.CodeDiscount,
                                               NameCourse = c.Name,
                                               IdDiscount = d.Id,
                                               IdCourse = c.Id,
                                               Image = c.Image
                                           }).ToListAsync();
            var discountHasNotCourse = await _context.Courses.Where(x => !discountHasCourse.Select(m => m.IdCourse).Any(m => x.Id.Equals(m))).ToListAsync();
            vm.lstDiscountCourse = discountHasCourse;
            vm.lstCourse = discountHasNotCourse;
            return Json(new { data = vm, status = vm.lstDiscountCourse.Count > 0 ? true : false });
        }
        public IActionResult Detail(int id)
        {
            DiscountDetailVM vm = new DiscountDetailVM();
            var lstParent = _courseCategoryService.GetParentCategory();
            vm.lstCategories = new SelectList(lstParent, "Id", "Name");
            vm.lstChildCategories = null;
            vm.ID = id;
            return View("Detail", vm);
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
        public IActionResult _Index(string searchDiscount, int page, int pageSize = 3)
        {
            var model = new Object();
            int totalRow = 0;
            if (string.IsNullOrEmpty(searchDiscount))
            {
                model = _discountService.GetPaging(null, out totalRow, page, pageSize, x => x.FromDate).Select(m => new
                {
                    m.Id,
                    m.CodeDiscount,
                    m.DiscountAmount,
                    m.DiscountPercent,
                    m.FromDate,
                    m.ToDate
                });
            }
            else
            {
                //model = _discountService.GetPaging(m => m.CodeDiscount.Contains(searchDiscount), out totalRow, page, pageSize, x => x.FromDate).
                //    Select(m => new
                //    {
                //        //m.Id,
                //        //m.Name,
                //        //m.Image,
                //        //m.Price,
                //        //m.PromotionPrice,
                //        //m.Status,
                //        //m.CategoryId
                //    }).ToList(); ;
            }
            return Json(new
            {
                data = model,
                total = totalRow,
                status = totalRow == 0 ? false : true
            });
        }
        public async Task<IActionResult> ApplyDiscountCourse(List<int> lstCourseHasChoose, int idDiscount)
        {
            quanlykhoahocContext context = _courseService.GetContext();
            bool status = false;
            using (IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                foreach (var item in lstCourseHasChoose)
                {
                    try
                    {
                        DiscountCourse discountCourse = new DiscountCourse();
                        discountCourse.Idcourse = item;
                        discountCourse.Iddiscount = idDiscount;
                        await _discountCourseService.CreateAsync(discountCourse);
                    }
                    catch
                    {
                        status = false;
                        transaction.Rollback();
                    }
                }
                status = true;
                transaction.Commit();
            }
            return Json(new { status = status });
        }
        public IActionResult Create(int ID)
        {
            var vm = new DiscountVM();
            if (ID != 0)
            {
                var model = _discountService.GetById(ID);
                vm = _mapper.Map<DiscountVM>(model);
            }
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(3145728)]
        public async Task<IActionResult> Create(DiscountVM vm)
        {
            if (ModelState.IsValid)
            {
                Discount model = _mapper.Map<Discount>(vm);
                await _discountService.CreateAsync(model);
            }
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
        [HttpPost]
        public async Task<IActionResult> DeleteDetail(List<int> lstCourseHasChoose, int idDiscount)
        {
            quanlykhoahocContext context = _courseService.GetContext();
            bool status = false;
            using (IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                foreach (var item in lstCourseHasChoose)
                {
                    try
                    {
                        DiscountCourse discountCourse = new DiscountCourse();
                        discountCourse.Idcourse = item;
                        discountCourse.Iddiscount = idDiscount;
                        await _discountCourseService.Delete(discountCourse);
                    }
                    catch
                    {
                        status = false;
                        transaction.Rollback();
                    }
                }
                status = true;
                transaction.Commit();
            }
            return Json(new { status = status });
        }
    }
}