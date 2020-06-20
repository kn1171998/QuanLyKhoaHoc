using System;
using System.Collections.Generic;

namespace WebData.Models
{
    public partial class Cart
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }

        public Courses Course { get; set; }
        public Users User { get; set; }
    }
}
