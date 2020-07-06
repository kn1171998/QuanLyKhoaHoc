using System;
using System.Collections.Generic;

namespace WebData.Models
{
    public partial class LessonComments
    {
        public int UserId { get; set; }
        public string Content { get; set; }
        public int LessonId { get; set; }
        public int? Report { get; set; }

        public CourseLessons Lesson { get; set; }
        public Users User { get; set; }
    }
}
