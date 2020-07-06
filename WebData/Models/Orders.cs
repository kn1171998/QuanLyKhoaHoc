using System;
using System.Collections.Generic;

namespace WebData.Models
{
    public partial class Orders
    {
        public string Id { get; set; }
        public int? UserId { get; set; }
        public string PayMethod { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public long TotalAmount { get; set; }
        public string Signature { get; set; }
        public string RequestId { get; set; }

        public Users User { get; set; }
    }
}
