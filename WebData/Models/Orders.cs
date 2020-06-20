using System;
using System.Collections.Generic;

namespace WebData.Models
{
    public partial class Orders
    {
        public Orders()
        {
            OrderDetails = new HashSet<OrderDetails>();
        }

        public int Id { get; set; }
        public int? UserId { get; set; }
        public byte[] PayMethod { get; set; }
        public DateTime? OrderDate { get; set; }
        public bool? Status { get; set; }
        public decimal? TotalAmount { get; set; }

        public Users User { get; set; }
        public ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
