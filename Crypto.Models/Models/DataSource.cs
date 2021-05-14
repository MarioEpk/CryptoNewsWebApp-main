using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crypto.Models;

namespace Crypto.WebApplication.Models
{
    public class DataSource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Post> Posts { get; set; }
        public DateTime CreatedAt { get; set; }
        public string TypeOfSource { get; set; }
        public CMCCoin Coin { get; set; }

        
    }
}
