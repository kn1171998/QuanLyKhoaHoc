using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebModel
{
    [Table("OrderDetails")]
    public class OrderDetail
    {
        [Key, Column(Order = 0)]
        [Required]
        [ForeignKey("OrderDetails_Orders")]
        public decimal OrderId { get; set; }
        public virtual Order Order { get; set; }
        [Key, Column(Order = 1)]
        [Required]
        [ForeignKey("OrderDetails_Courses")]
        public decimal CourseId { get; set; }
        public virtual Course Course { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }
        [ForeignKey("OrderDetails_Discounts")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountId { get; set; }
        public virtual Discount Discount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
    }
}
