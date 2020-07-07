using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebData.Models;

namespace DoAnTotNghiep.Models
{
    public class DashboardVM
    {
        public DashboardVM()
        {           
        }
        public int TotalCustomerDayNow { get; set; }
        public long RevenueToDay { get; set; }
        public int TotalCourseDayNow { get; set; }
    }
}
