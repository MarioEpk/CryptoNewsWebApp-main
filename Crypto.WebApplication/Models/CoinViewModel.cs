using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crypto.WebApplication.Models
{
    public class CoinViewModel
    {
        public string Name { get; set; }
        public float LatestPrice { get; set; }
        public float[] Prices { get; set; }
        public int CMCRank { get; set; }
    }
}
