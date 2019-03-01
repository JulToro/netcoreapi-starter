using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netcore.api.Models
{
    public class RootDto<T>
    {
        public List<LinkDto> _links { get; set; }
        public List<T> items { get; set; }
    }
}
