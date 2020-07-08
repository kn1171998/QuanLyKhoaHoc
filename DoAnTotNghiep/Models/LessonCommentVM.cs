using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnTotNghiep.Models
{
    public class LessonCommentVM
    {
        public long UserId { get; set; }
        public int LessonId { get; set; }        
        public string Content { get; set; }
        public int Report { get; set; }
        public string FullName { get; set; }
        public string ImageUrl { get; set; }
    }
}