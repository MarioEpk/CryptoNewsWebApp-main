using CryptoNewsWebApp.Data;
using CryptoNewsWebApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Net;
using System.Web;
using WebApplication.Models;
using Newtonsoft.Json;
using System.Linq;

namespace DataAPIRequests.APIClients
{
    public class MarketCapClient : IDataAccess
    {
        private const string CREDENTIALS_FILE_PATH = @"C:\Users\Epakf\source\repos\CryptoNewsWebApp-main\DataAPIRequests\CoinMarketCapCredentials.txt";
        private const string API_RESPONSE_FILE_PATH = @"C:\Users\Epakf\source\repos\CryptoNewsWebApp-main\Tests\APIResponse.txt";
        private const string API_ENDPOINT = "https://pro-api.coinmarketcap.com/v1/cryptocurrency/quotes/latest";

        private string coinMarketcapApiKey;

        ApplicationDbContext _context;

        public MarketCapClient(ApplicationDbContext context)
        {
            _context = context;

            try
            {
                LoadCredentials();
            }
            catch (TimeoutException exception)
            {
                throw exception;
            }
        }

        public DataSource LoadData()
        {
            var response = JsonConvert.DeserializeObject<ClientResponse>(GetCardanoJson(coinMarketcapApiKey));

            DataSource cardanoCoin = new DataSource();
            cardanoCoin.Coin = new CMCCoin();
            cardanoCoin.Coin.Name = response.Data.Cardano.name;
            cardanoCoin.Coin.CMCRank = response.Data.Cardano.cmc_rank;
            cardanoCoin.Coin.CreatedAt = DateTime.Now;
            cardanoCoin.Coin.Price = response.Data.Cardano.quote.USD.price;

            return cardanoCoin;
        }

        private string GetCardanoJson(string apiKey)
        {
            var URL = new UriBuilder(API_ENDPOINT);

            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["id"] = "2010";
            queryString["convert"] = "USD";

            URL.Query = queryString.ToString();

            var client = new WebClient();
            client.Headers.Add("X-CMC_PRO_API_KEY", apiKey);
            client.Headers.Add("Accepts", "application/json");
            return client.DownloadString(URL.ToString());
        }

        public async Task SaveDataToDatabase(DataSource source)
        {
            await _context.AddAsync<DataSource>(source);
            await _context.SaveChangesAsync();

            Console.WriteLine("CMC data saved into the database.");
        }

        private void LoadCredentials()
        {
            string FILE_PATH = @"C:\Users\Epakf\source\repos\CryptoNewsWebApp-main\DataAPIRequests\CoinMarketCapCredentials.txt";

            try
            {
                var credentials = File.ReadAllLines(FILE_PATH);
                this.coinMarketcapApiKey = credentials[0];
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("The file or directory cannot be found.");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("You do not have permission to access this file.");
            }
            catch (IOException)
            {
                Console.WriteLine("An unexpected error occured while loading the Reddit app credentials");
            }
        }
    }

}

