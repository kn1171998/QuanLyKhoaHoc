using System.Collections.Generic;
using WebData.Models;

namespace DoAnTotNghiep.Models
{
    public class CourseCategoryVM
    {
        public CourseCategoryVM()
        {

        }

        public IEnumerable<CourseCategories> listCourseCategory { get; set; }
        public long ID { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public string SeoAlias { get; set; }
        public string SeoMetakeywords { get; set; }
        public string SeoTitle { get; set; }
        public long ParentId { get; set; }
        public bool Status { get; set; }
        public string SearchName { get; set; }
    }
}