using System;
using System.Collections.Generic;

namespace WebData.Models
{
    public partial class CourseLessons
    {
        public CourseLessons()
        {
            LessonComments = new HashSet<LessonComments>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string VideoPath { get; set; }
        public string SlidePath { get; set; }
        public string TypeDocument { get; set; }
        public int? SortOrder { get; set; }
        public bool? Status { get; set; }
        public int? ChapterId { get; set; }

        public Chapter Chapter { get; set; }
        public ICollection<LessonComments> LessonComments { get; set; }
    }
}
