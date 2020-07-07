using System;

namespace DoAnTotNghiep.Models
{
    public class DiscountVM
    {
        public long ID { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime FromDate { get; set; }
        public string CodeDiscount { get; set; }
        public int CheckTypeDiscount { get; set; }
        public int DiscountPercent { get; set; }
        public long DiscountAmount { get; set; }
        public string NameListCourse { get; set; }
        public string SearchName { get; set; }
    }
}