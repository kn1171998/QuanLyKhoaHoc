using System;
using System.Collections.Generic;

namespace WebData.Models
{
    public partial class MasterList
    {
        public MasterList()
        {
            Orders = new HashSet<Orders>();
            Users = new HashSet<Users>();
        }

        public string MasterListCode { get; set; }
        public string MasterListGroupCde { get; set; }
        public string MasterListDefaultValue { get; set; }
        public string Description { get; set; }

        public ICollection<Orders> Orders { get; set; }
        public ICollection<Users> Users { get; set; }
    }
}
