using System;
using System.Collections.Generic;

namespace WebData.Models
{
    public partial class Discount
    {
        public Discount()
        {
            DiscountCourse = new HashSet<DiscountCourse>();
            OrderDetails = new HashSet<OrderDetails>();
        }

        public int Id { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime FromDate { get; set; }
        public string CodeDiscount { get; set; }
        public int? DiscountPercent { get; set; }
        public long? DiscountAmount { get; set; }
        public int? IdcategoryAll { get; set; }
        public bool? IsAll { get; set; }

        public ICollection<DiscountCourse> DiscountCourse { get; set; }
        public ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
