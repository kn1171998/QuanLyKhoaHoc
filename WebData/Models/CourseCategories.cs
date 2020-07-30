using System;
using System.Collections.Generic;

namespace WebData.Models
{
    public partial class CourseCategories
    {
        public CourseCategories()
        {
            Courses = new HashSet<Courses>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? SortOrder { get; set; }
        public int? ParentId { get; set; }
        public bool? Status { get; set; }

        public ICollection<Courses> Courses { get; set; }
    }
}
