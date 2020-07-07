using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using WebData.Models;

namespace DoAnTotNghiep.Models
{
    public class CourseLessonVM
    {
        public CourseLessonVM()
        {            
        }    
        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile Video { get; set; }
        public string VideoDisplay { get; set; }
        public string VideoPath { get; set; }
        public IFormFile Slide { get; set; }
        public string SlideDisplay { get; set; }
        public string SlidePath { get; set; }
        public string Attachment { get; set; }
        public int? SortOrder { get; set; }
        public bool? Status { get; set; }
        public int? ChapterId { get; set; }
    }
}