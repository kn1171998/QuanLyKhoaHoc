using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebModel
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID { get; set; }
        [ForeignKey("Order_Users")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UserId { get; set; }
        public virtual User User { get; set; }
        [ForeignKey("Order_MasterList")]
        public string PayMethod { get; set; }
        public virtual MasterList MasterList { get; set; }
        public DateTime OrderDate { get; set; }
        public bool Status { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
