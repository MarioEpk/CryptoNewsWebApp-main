using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crypto.WebApplication.Models
{
    public class PostViewModel
    {
        public string Title { get; set; }
        public string URL { get; set; }
        public DateTime PostedAt { get; set; }
    }
}
