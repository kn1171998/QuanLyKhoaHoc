using System;
using System.Collections.Generic;

namespace WebData.Models
{
    public partial class OrderDetails
    {
        public int OrderId { get; set; }
        public int CourseId { get; set; }
        public decimal? Quantity { get; set; }
        public int? DiscountId { get; set; }
        public decimal? Amount { get; set; }

        public Courses Course { get; set; }
        public Discount Discount { get; set; }
        public Orders Order { get; set; }
    }
}
