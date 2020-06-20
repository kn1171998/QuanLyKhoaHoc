using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnTotNghiep.Models
{
    public class OrderVM
    {       
        public long ID { get; set; }
        public long UserId { get; set; }        
        public string PayMethod { get; set; }
        public DateTime OrderDate { get; set; }
        public bool Status { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
