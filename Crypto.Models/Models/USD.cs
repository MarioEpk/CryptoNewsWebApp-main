using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crypto.Models
{
    public class USD
    {
        [JsonProperty("price")]
        public float Price { get; set; }
        [JsonProperty("percent_change_1h")]
        public float Percent_change_1h { get; set; }
        [JsonProperty("percent_change_24h")]
        public float Percent_change_24h { get; set; }
        [JsonProperty("percent_change_7d")]
        public float Percent_change_7d { get; set; }
    }
}
