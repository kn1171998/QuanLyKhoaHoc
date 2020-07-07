using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebModel
{
    [Table("CourseLessons")]
    public class CourseLesson
    {
        [Key]
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID { get; set; }
        public string NameLesson { get; set; }
        public string VideoPath { get; set; }
        public string SlidePath { get; set; }
        public string Attachment { get; set; }
        public int SortOrder { get; set; }
        public bool Status { get; set; }
        [ForeignKey("CourseLesson_Course")]
        [Column(TypeName = "decimal(18,2)")] 
        public decimal CourseId { get; set; }
        public virtual Course Course { get; set; }
        public string Chapter { get; set; }
        public int OrderChapter { get; set; }
        public virtual ICollection<LessonComment> LessonComments { get; set; }
    }
}