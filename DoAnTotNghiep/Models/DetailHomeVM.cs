using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebData.Models;

namespace DoAnTotNghiep.Models
{
    public class DetailHomeVM
    {
        public DetailHomeVM()
        {
            lstCourseLesson = new List<CourseLessons>();
            lstChapter = new List<Chapter>();
        }
        public List<CourseLessons> lstCourseLesson { get; set; }
        public List<Chapter> lstChapter { get; set; }
        public SelectList lstCategories { get; set; }
        public SelectList lstChildCategories { get; set; }
        public int IdCourse { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Content { get; set; }
        public long Price { get; set; }
        public long? PromotionPrice { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsFree { get; set; }
        public bool HasBuy { get; set; }
        public bool Status { get; set; }
        public int CategoryId { get; set; }
        public string NameCategory { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
    }
}
