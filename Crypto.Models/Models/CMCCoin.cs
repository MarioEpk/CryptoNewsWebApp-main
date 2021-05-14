using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crypto.Models
{

    /// <summary>
    /// DB model for CoinmarketCap coins
    /// </summary>
    public class CMCCoin
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int CMCRank { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
