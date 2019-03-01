using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netcore.api.Models
{
    public class ItemDto<T>
    {
        public List<LinkDto> _links { get; set; }
        public T data { get; set; }
    }
}
