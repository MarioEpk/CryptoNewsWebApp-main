using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Web;
using Crypto.Models;
using Newtonsoft.Json;
using System.Linq;
using Serilog;
using Crypto.Models.Data;

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
                CoinmarketcapCredentials credentials = CryptoHelper.LoadCoinmarketcapCredentials(_logger);

                if (credentials != null)
                {
                    coinMarketcapApiKey = credentials.coinmarketcapApiKey;
                } else
                {
                    throw new Exception("Could not load credentials.");
                }
            }
            catch (Exception exception)
            {
                _logger.Error(exception.Message);
                throw exception;
            }
        }

        public async Task<DataSource> LoadData()
        {
            _logger.Debug("Fetching data from the CMC API...");

            var json = await GetCardanoJson(coinMarketcapApiKey);
            var response = JsonConvert.DeserializeObject<ClientResponse>(json);

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

        private async Task<string> GetCardanoJson(string apiKey)
        {
            var url = new UriBuilder(API_ENDPOINT);

            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["id"] = "2010";
            queryString["convert"] = "USD";

            url.Query = queryString.ToString();

            var client = new WebClient();
            client.Headers.Add("X-CMC_PRO_API_KEY", apiKey);
            client.Headers.Add("Accepts", "application/json");

            return await client.DownloadStringTaskAsync(url.ToString());
        }

        public async Task SaveDataToDatabase(DataSource source)
        {
            await _context.AddAsync(source);
            await _context.SaveChangesAsync();

            _logger.Debug("CMC data saved into the database.");
        }

        public async Task ClearOldEntries()
        {
            var oldDataSourceEntry = _context.DataSource
                .Where(datasource => datasource.CreatedAt < DateTime.Now.AddMonths(-3));

            var oldCMCCoins = oldDataSourceEntry
                .Select(d => d.Coin);

            await oldDataSourceEntry.ForEachAsync(datasource =>
            {
                _context.Remove(datasource);
            });

            _logger.Debug("Old datasource entries deleted from database");

            await oldCMCCoins.ForEachAsync(coin =>
            {
                _context.Remove(coin);
            });

            await _context.SaveChangesAsync();
            _logger.Debug("Old CMC coins deleted from database");
        }
    }
}

