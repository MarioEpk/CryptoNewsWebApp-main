using Newtonsoft.Json;
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
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("cmc_rank")]
        public int Cmc_rank { get; set; }
        [JsonProperty("quote")]
        public Quote Quote { get; set; }
        
    }
}
