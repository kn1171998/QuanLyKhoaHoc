using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebModel
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID { get; set; }
        [ForeignKey("TypeUser_MasterList")]
        [Required]
        [StringLength(50)]
        public string TypeUser { get; set; }
        public virtual MasterList MasterList { get; set; }
        [Required]
        [StringLength(255)]
        public string FullName { get; set; }
        public bool Sex { get; set; }
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string  Token { get; set; }
        public string Introduction { get; set; }
        public string ImageUrl { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual ICollection<LessonComment> LessonComments { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}
