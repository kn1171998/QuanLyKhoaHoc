using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnTotNghiep.Models
{
    public class LessonCommentVM
    {
        public long UserId { get; set; }
        public long CourseId { get; set; }        
        public string Content { get; set; }
        public int Report { get; set; }
    }
}