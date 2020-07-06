using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebModel
{
    [Table("Cart")]
    public class Cart
    {
        [Key,Column(Order=0)]
        [Required]
        [ForeignKey("Carts_Users")]
        public decimal UserId { get; set; }
        public virtual User User { get; set; }
        [Key,Column(Order=1)]
        [Required]
        [ForeignKey("Carts_Courses")]    
        public decimal CourseId { get; set; }
        public virtual Course Course { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}
