using System;
using System.Collections.Generic;

namespace WebData.Models
{
    public partial class Discount
    {
        public Discount()
        {
            OrderDetails = new HashSet<OrderDetails>();
        }

        public int Id { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? FromDate { get; set; }
        public string CodeDiscount { get; set; }
        public decimal? DiscountPercent { get; set; }
        public decimal? DiscountAmount { get; set; }
        public int? CouresId { get; set; }

        public Courses Coures { get; set; }
        public ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
