using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class Root
    {
        public List<Item> items { get; set; }
        public bool has_more { get; set; }
        public int backoff { get; set; }
        public int quota_max { get; set; }
        public int quota_remaining { get; set; }
    }
}
