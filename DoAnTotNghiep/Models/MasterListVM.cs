using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnTotNghiep.Models
{
    public class MasterListVM
    {
        public string MasterListCode { get; set; }
        public string MasterListGroupCde { get; set; }
        public string MasterListDefaultValue { get; set; }
        public string Description { get; set; }
    }
}
