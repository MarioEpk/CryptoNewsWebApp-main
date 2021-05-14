using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Crypto.Models
{
    [Serializable]
    public class Coin
    {
        public int Id { get; set; }
        public string name { get; set; }
        public int cmc_rank { get; set; }
        public Quote quote { get; set; }
        
    }
}
