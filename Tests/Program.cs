using CoinMarketCap.Models.Cryptocurrency;
using System;
using System.IO;
using System.Threading;
using System.Linq;
using System.Web;
using System.Net;
using Newtonsoft.Json;
using WebApplication.Models;
using System.Collections.Generic;

namespace Tests
{
    class Program
    {
        private const string CREDENTIALS_FILE_PATH = @"C:\Users\Epakf\source\repos\CryptoNewsWebApp-main\DataAPIRequests\CoinMarketCapCredentials.txt";
        private const string API_RESPONSE_FILE_PATH = @"C:\Users\Epakf\source\repos\CryptoNewsWebApp-main\Tests\APIResponse.txt";
        private const string API_ENDPOINT = "https://pro-api.coinmarketcap.com/v1/cryptocurrency/quotes/latest";


        public static void Main(string[] args)
        {

            var credentials = File.ReadAllLines(CREDENTIALS_FILE_PATH);
            var apiKey = credentials[0];

            //Coin cardanoResponse = JsonConvert.DeserializeObject<Coin>(GetCardanoJson(apiKey));

            //File.WriteAllText(API_RESPONSE_FILE_PATH, GetCardanoJson(apiKey));

            var cardanoJsonFile = File.ReadAllText(API_RESPONSE_FILE_PATH);
            //var cardanoJson = JsonConvert.DeserializeObject<dynamic>(cardanoJsonFile);
            //var stringCardanoJson = cardanoJson["data"]["2010"].ToString();

            //Coin cardano = JsonConvert.DeserializeObject<Coin>(stringCardanoJson);

            var response = JsonConvert.DeserializeObject<ClientResponse>(cardanoJsonFile);

            

            //var cardanoJson = jsonResponse["data"];
            // je tu nejaky problem s tym text jsonom, nejak sa to nevie parsnut do modelu
            //cardanoJson = cardanoJson["2010"];


            Console.WriteLine(response.Data.Cardano.name);
            Console.WriteLine(response.Data.Cardano.cmc_rank);
            Console.WriteLine(response.Data.Cardano.quote.USD.price);
            Console.WriteLine("done");
        }
        static string GetCardanoJson(string apiKey)
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
    }

}
