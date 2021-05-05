using CryptoNewsWebApp.Data;
using CryptoNewsWebApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CoinMarketCap;

namespace DataAPIRequests.APIClients
{
    public class MarketCapClient : IDataAccess
    {

        private CoinMarketCapClient client;
        private string coinMarketcapApiKey;
        ApplicationDbContext _context;
        public MarketCapClient(ApplicationDbContext context)
        {
            _context = context;
            try
            {
                LoadCredentials();
                // Prerobit Client na autofac
                client = new CoinMarketCapClient(coinMarketcapApiKey);
            }
            catch (TimeoutException exception)
            {
                throw exception;
            }
        }

        public DataSource LoadData()
        {
            DataSource cardanoCoin;

            throw new NotImplementedException();
            // Dal som si do bookmarky coinmarketcapAPI examples.
        }

        public Task SaveDataToDatabase(DataSource source)
        {
            throw new NotImplementedException();
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

