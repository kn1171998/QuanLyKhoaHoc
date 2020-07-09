using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebData.Models;

namespace DoAnTotNghiep.Models
{
    public class OrderVM
    {
        public OrderVM()
        {
            //lstOrder = new List<Orders>();
        }
        public IEnumerable<Orders> lstOrder { get; set; }
        public string ID { get; set; }
        public int? UserId { get; set; }
        public string PayMethod { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public long TotalAmount { get; set; }
        public string SearchName { get; set; }
    }
}
