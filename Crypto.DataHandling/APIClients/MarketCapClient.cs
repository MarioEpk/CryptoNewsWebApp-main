using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Web;
using Crypto.Models;
using Newtonsoft.Json;
using System.Linq;
using Serilog;
using Crypto.Models.Data;
using Newtonsoft.Json.Linq;

namespace Crypto.DataHandling.APIClients
{
    public class MarketCapClient : IDataAccess
    {
        private const string API_ENDPOINT = "https://pro-api.coinmarketcap.com/v1/cryptocurrency/quotes/latest";
        private string coinMarketcapApiKey;

        // DI
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public MarketCapClient(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;

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
            _logger.Debug("Fetching data from the CMC API...");

            var marketcapCredentials = File.ReadAllText((Path.Combine(Directory.GetCurrentDirectory(), "Credentials.json")));
            var response = JsonConvert.DeserializeObject<ClientResponse>(GetCardanoJson(coinMarketcapApiKey));

            CMCCoin coin = new CMCCoin
            {
                Name = response.Data.Cardano.Name,
                CMCRank = response.Data.Cardano.Cmc_rank,
                CreatedAt = DateTime.Now,
                Price = response.Data.Cardano.Quote.USD.Price
            };

            DataSource cardanoCoin = new DataSource
            {
                Coin = coin,
                TypeOfSource = "CMC",
                CreatedAt = DateTime.Now,
                Name = "CardanoCMC"
            };

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
            await _context.AddAsync(source);
            await _context.SaveChangesAsync();

            _logger.Debug("CMC data saved into the database.");
        }

        private void LoadCredentials()
        {
            try
            {
                var marketCapCredentials = File.ReadAllText((Path.Combine(Directory.GetCurrentDirectory(), "Credentials.json")));
                Credentials credentials = JsonConvert.DeserializeObject<Credentials>(marketCapCredentials);

                coinMarketcapApiKey = credentials.coinmarketcapCredentials.coinmarketcapApiKey;
            }
            catch (FileNotFoundException)
            {
                _logger.Error("The file or directory cannot be found.");
            }
            catch (UnauthorizedAccessException)
            {
                _logger.Error("You do not have permission to access this file.");
            }
            catch (IOException)
            {
                _logger.Error("An unexpected error occured while loading the Reddit app credentials");
            }
        }

        public async Task ClearOldEntries()
        {
            var oldDataSourceEntry = _context.DataSource.Where(datasource => datasource.CreatedAt < DateTime.Now.AddMonths(-1));

            await oldDataSourceEntry.ForEachAsync(datasource =>
            {
                _context.Remove(datasource);
            });

            _logger.Debug("Old datasource entries deleted from database");


            var oldCMCCoins = _context.Coin.Where(coin => coin.CreatedAt < DateTime.Now.AddMonths(-1));
            await oldCMCCoins.ForEachAsync(coin =>
            {
                _context.Remove(coin);
            });

            await _context.SaveChangesAsync();
            _logger.Debug("Old CMC coins deleted from database");
        }
    }
}

