using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebModel
{
    [Table("Courses")]
    public class Course
    {
        [Key]
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID { get; set; }
        [Required]
        public string NameCourse { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Content { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PromotionPrice { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Status { get; set; }
        [ForeignKey("Courses_CategoryCourses")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CategoryId { get; set; }
        public virtual CourseCategory CourseCategory { get; set; }
        [ForeignKey("Courses_Users")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Discount> Discounts { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<CourseLesson> CourseLessons { get; set; }
    }
}
