using Newtonsoft.Json;

namespace Crypto.Models
{
    // Specific coins/tokens are represented by their ID's, for example '2010'(cardano)
    public class CoinData
    {
        
        [JsonProperty("2010")] 
        public Coin Cardano { get; set; }
    }
}
