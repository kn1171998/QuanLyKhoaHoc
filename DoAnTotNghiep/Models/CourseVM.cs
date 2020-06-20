using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebData.Models;

namespace DoAnTotNghiep.Models
{
    public class CourseVM
    {
        public CourseVM()
        {
            lstCourseLesson = new List<CourseLessons>();
        }
        public List<CourseLessons> lstCourseLesson { get; set; }
        //public IEnumerable<CourseCategories> lstCategories { get; set; }
        public SelectList lstCategories { get; set; }
        public SelectList lstChildCategories { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        [Required(ErrorMessage = "Vui lòng không để trống")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        public long? Price { get; set; }
        public long? PromotionPrice { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Status { get; set; }
        public int CategoryId { get; set; }        
        public int UserId { get; set; }
        public string SearchCourse { get; set; }
    }
}
