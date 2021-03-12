using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class Answer
    {
        public List<Comment> comments { get; set; }
        public int score { get; set; }
        public int creation_date { get; set; }
        public int question_id { get; set; }
        public string body { get; set; }
    }
}
