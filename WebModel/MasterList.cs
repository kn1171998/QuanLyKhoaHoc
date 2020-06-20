using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebModel
{
    [Table("MasterList")]
    public class MasterList
    {
        [Key]
        [Required]
        [StringLength(50)]
        public string MasterListCode { get; set; }
        [StringLength(50)]
        public string MasterListGroupCde { get; set; }
        [StringLength(255)]
        public string MasterListDefaultValue { get; set; }
        public string Description { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<CourseLesson> CourseLessons { get; set; }
    }
}
