using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnTotNghiep.Models
{
    public class OrderDetailVM
    {
        public long OrderId { get; set; }
        public long CourseId { get; set; }
        public decimal Quantity { get; set; }
        public long DiscountId { get; set; }
        public decimal Amount { get; set; }
    }
}
