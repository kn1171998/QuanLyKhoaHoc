using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebModel
{
    [Table("LessonComments")]
    public class LessonComment
    {
        [Key, Column(Order = 0)]
        [Required]
        [ForeignKey("LessonComment_User")]
        public decimal UserId { get; set; }
        public virtual User User { get; set; }
        [Key, Column(Order = 1)]
        [Required]
        [ForeignKey("LessonComment_Courses")]
        public decimal CourseId { get; set; }
        public virtual Course Course { get; set; }
        public string Content { get; set; }
        public int Report { get; set; }
    }
}