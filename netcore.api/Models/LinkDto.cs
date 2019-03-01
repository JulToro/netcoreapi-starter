using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netcore.api.Models
{
    public class LinkDto
    {
        public string href { get; set; }
        public string rel { get; set; }
        public string method { get; set; }
    }
}
