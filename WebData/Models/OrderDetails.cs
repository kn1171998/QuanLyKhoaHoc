using System;
using System.Collections.Generic;

namespace WebData.Models
{
    public partial class OrderDetails
    {
        public string OrderId { get; set; }
        public int CourseId { get; set; }
        public long? Quantity { get; set; }
        public int? DiscountId { get; set; }
        public long? Amount { get; set; }

        public Courses Course { get; set; }
        public Discount Discount { get; set; }
    }
}
