using Crypto.Models.Models;
using System.Collections.Generic;

namespace Crypto.WebApplication.Models
{
    public class CoinViewModel
    {
        public string Name { get; set; }
        public float LatestPrice { get; set; }
        public IEnumerable<ChartItem> Prices { get; set; }
        public int CMCRank { get; set; }
    }
}
