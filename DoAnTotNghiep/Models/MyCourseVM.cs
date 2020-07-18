using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebData.Models;

namespace DoAnTotNghiep.Models
{   
    public class MyCourseVM
    {
        public MyCourseVM()
        {
        }
        public IEnumerable<CourseVM> listCourse { get; set; }
        public string  SearchName { get; set; }
    }
}