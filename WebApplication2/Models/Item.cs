using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class Item
    {
        public List<string> tags { get; set; }
        public List<Comment> comments { get; set; }
        public List<Answer> answers { get; set; }
        public int score { get; set; }
        public int creation_date { get; set; }
        public string link { get; set; }
        public string title { get; set; }
        public string body { get; set; }

      
    }
}
