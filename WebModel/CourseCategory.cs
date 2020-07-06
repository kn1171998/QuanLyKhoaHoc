using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebModel
{
    [Table("CourseCategories")]
    public class CourseCategory
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "decimal(18,2)")]  
        public decimal ID { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public string SeoAlias { get; set; }
        public string SeoMetakeywords { get; set; }
        public string SeoTitle { get; set; }        
        [Column(TypeName = "decimal(18,2)")]
        public decimal ParentId { get; set; }
        public CourseCategory Parent { get; set; }
        public ICollection<CourseCategory> Children { get; set; }
        public bool Status { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}