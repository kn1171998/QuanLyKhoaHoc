using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using WebData.Models;

namespace DoAnTotNghiep.Models
{
    public class DiscountDetailVM
    {
        public DiscountDetailVM()
        {
            lstDiscountCourse = new List<DiscountDetailVM>();
            lstCourse = new List<Courses>();
        }
        public List<DiscountDetailVM> lstDiscountCourse { get; set; }
        public List<Courses> lstCourse { get; set; }
        public SelectList lstCategories { get; set; }
        public SelectList lstChildCategories { get; set; }
        public long ID { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime FromDate { get; set; }
        public string CodeDiscount { get; set; }
        public int CheckTypeDiscount { get; set; }
        public int DiscountPercent { get; set; }
        public long DiscountAmount { get; set; }
        public string NameListCourse { get; set; }
        public string NameCourse { get; set; }
        public int IdCourse { get; set; }
        public int IdDiscount { get; set; }
        public string Image { get; set; }
        public string SearchName { get; set; }
        public string CategoryParent { get; set; }
        public string CategoryId { get; set; }
    }
}