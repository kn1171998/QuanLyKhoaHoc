using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebModel
{
    [Table("Discounts")]
    public class Discount
    {
        [Key]
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime FromDate { get; set; }
        public string CodeDiscount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountPercent { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; }
        [ForeignKey("Discounts_Courses")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CourseId { get; set; }
        public virtual Course Course { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
