using System;
using System.Collections.Generic;

namespace WebData.Models
{
    public partial class Sysdiagrams
    {
        public int PrincipalId { get; set; }
        public int DiagramId { get; set; }
        public int? Version { get; set; }
        public byte[] Definition { get; set; }
    }
}
