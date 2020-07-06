using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnTotNghiep.Models
{

    public class CartVM
    {   
        public long UserId { get; set; }
        public long CourseId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
