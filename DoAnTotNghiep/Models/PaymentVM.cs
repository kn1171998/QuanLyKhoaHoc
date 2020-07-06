using System.Collections.Generic;
using WebData.Models;

namespace DoAnTotNghiep.Models
{
    public class PaymentVM
    {
        public PaymentVM()
        {
            lstCourse = new List<Courses>();
        }

        public List<Courses> lstCourse { get; set; }
        public long TotalMoney { get; set; }
        public string Discount { get; set; }
        public decimal DiscountMoney { get; set; }
        public long SumMoney { get; set; }
        public string PaymentMethod { get; set; }
    }
}