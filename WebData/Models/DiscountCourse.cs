using System;
using System.Collections.Generic;

namespace WebData.Models
{
    public partial class DiscountCourse
    {
        public int Iddiscount { get; set; }
        public int Idcourse { get; set; }

        public Courses IdcourseNavigation { get; set; }
        public Discount IddiscountNavigation { get; set; }
    }
}
