using System;
using System.Collections.Generic;

namespace WebData.Models
{
    public partial class Chapter
    {
        public Chapter()
        {
            CourseLessons = new HashSet<CourseLessons>();
        }

        public int Id { get; set; }
        public string NameChapter { get; set; }
        public int? CourseId { get; set; }
        public int? OrderChapter { get; set; }

        public Courses Course { get; set; }
        public ICollection<CourseLessons> CourseLessons { get; set; }
    }
}
