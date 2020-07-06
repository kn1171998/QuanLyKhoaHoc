using AutoMapper;
using DoAnTotNghiep.Models;
using Microsoft.AspNetCore.Authorization;
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
    public class DashboardController : Controller
    {
        private readonly ICourseService _courseService;
        public readonly IMapper _mapper;
        public readonly IOrderService _orderService;
        public quanlykhoahocContext _context;
        public DashboardController(IMapper mapper,
                                   ICourseService courseService,
                                   IOrderService orderService)
        {
            _orderService = orderService;
            _courseService = courseService;
            _mapper = mapper;
            _context = _courseService.GetContext();
        }
        public IActionResult ReportRevenue()
        {
            return View();
        }
        [Authorize(Roles ="Admin,User")]
        public IActionResult Index()
        {
            DashboardVM dashboard = new DashboardVM();
            DateTime daynow = DateTime.Now;
            var orderDay = _orderService.GetCondition(m => m.OrderDate.Date == daynow.Date);
            dashboard.TotalCustomerDayNow = orderDay.Select(m => m.UserId).Distinct().Count();
            dashboard.RevenueToDay = orderDay.Sum(m => m.TotalAmount);
            dashboard.TotalCourseDayNow = (from o in _context.Orders
                                           join od in _context.OrderDetails
                                           on o.Id equals od.OrderId
                                           join c in _context.Courses
                                           on od.CourseId equals c.Id
                                           where o.OrderDate.Date == daynow.Date
                                           select c.Id).Distinct().Count();
            return View(dashboard);
        }
        [HttpPost]
        public IActionResult Report(DateTime startdate, DateTime enddate)
        {
            var TotalCourseDateRange = (from o in _context.Orders
                                        join od in _context.OrderDetails
                                        on o.Id equals od.OrderId
                                        join c in _context.Courses
                                        on od.CourseId equals c.Id
                                        where o.OrderDate.Date >= startdate.Date
                                              && o.OrderDate.Date <= enddate.Date
                                        select c.Id).Distinct().Count();
            var orderRange = _orderService.GetCondition(m => m.OrderDate.Date >= startdate.Date
                                                             && m.OrderDate.Date <= enddate.Date);
            var TotalCustomerDateRange = orderRange.Select(m => m.UserId).Distinct().Count();
            var RevenueDateRange = orderRange.Sum(m => m.TotalAmount);
            return Json(new { totalcourse = TotalCourseDateRange, totalcustomer = TotalCustomerDateRange, revenue = RevenueDateRange });
        }
    }
}