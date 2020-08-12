using AutoMapper;
using DoAnTotNghiep.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebData.Implementation;
using WebData.Models;

namespace DoAnTotNghiep.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _IOrderService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _hostingEnvironment;

        public OrderController(IOrderService IOrderService,
                              IHostingEnvironment hostingEnvironment,
                              IUserService userService,
                              IMapper mapper)
        {
            _hostingEnvironment = hostingEnvironment;
            _IOrderService = IOrderService;
            _userService = userService;
            _mapper = mapper;
        }
        [Authorize(Roles = "Admin,Teacher")]
        public IActionResult Index()
        {
            var orderVM = new OrderVM();
            return View(orderVM);
        }

        [HttpGet]
        public IActionResult _Index(string searchName, int page, int pageSize = 3)
        {
            var context = _IOrderService.GetContext();
            var model = new Object();
            int totalRow = 0;
            if (string.IsNullOrEmpty(searchName))
            {
                var model1 = _IOrderService.GetPaging(null, out totalRow, page, pageSize, x => x.OrderDate).
                   Select(m => new Orders
                   {
                       Id = m.Id,
                       OrderDate = m.OrderDate,
                       PayMethod = m.PayMethod,
                       TotalAmount = m.TotalAmount,
                       Status = m.Status,
                       UserId = m.UserId
                   });
                var a = from m in model1
                        join c in context.Users
                        on m.UserId equals c.Id
                        select new
                        {
                            m.Id,
                            c.FullName,
                            m.OrderDate,
                            m.PayMethod,
                            m.TotalAmount,
                            m.Status,
                        };
                model = a;
            }
            else
            {
                //  var user =
                var model1 = _IOrderService.GetPaging(null, out totalRow, page, pageSize, x => x.OrderDate).
                      Select(m => new
                      {
                          m.Id,
                          m.OrderDate,
                          m.PayMethod,
                          m.TotalAmount,
                          m.Status,
                          m.UserId
                      }).ToList();
                var a = from m in model1
                        join c in context.Users
                        on m.UserId equals c.Id
                        select new
                        {
                            m.Id,
                            c.FullName,
                            m.OrderDate,
                            m.PayMethod,
                            m.TotalAmount,
                            m.Status,
                        };
                model = a;
            }
            return Json(new
            {
                data = model,
                total = totalRow,
                status = true
            });
        }

        //public IActionResult Create(int ID)
        //{
        //    var vm = new OrderVM();
        //    if (ID == 0)
        //    {
        //        vm.lstOrder = _IOrderService.GetAll();
        //    }
        //    else
        //    {
        //        var modelcourse = _IOrderService.GetById(ID);
        //        if (modelcourse == null)
        //        {
        //            return NotFound();
        //        }
        //        vm = _mapper.Map<OrderVM>(modelcourse);
        //        vm.lstOrder = _IOrderService.GetAll();
        //    }

        //    return View(vm);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(UserVM vm)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            vm.Status = true;
        //            //Create
        //            if (vm.Id == 0)
        //            {
        //                var model = _mapper.Map<Orders>(vm);
        //                await _IOrderService.CreateAsync(model);
        //                ViewBag.IsSuccess = true;
        //                return RedirectToAction(nameof(Index));
        //            }
        //            else
        //            {
        //                var model = _mapper.Map<Orders>(vm);
        //                await _IOrderService.UpdateAsync(model);
        //                ViewBag.IsSuccess = true;
        //                return RedirectToAction(nameof(Index));
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ViewBag.IsSuccess = false;
        //            return View(vm);
        //        }
        //    }
        //    return View(vm);
        //}

        public async Task<IActionResult> Delete(string ID)
        {
            bool result = true;
            try
            {
                var order = _IOrderService.GetBySingle(ID);
                order.Status = order.Status == OrderStatus.Paid ? OrderStatus.Unpaid : OrderStatus.Paid;
                await _IOrderService.UpdateAsync(order);
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