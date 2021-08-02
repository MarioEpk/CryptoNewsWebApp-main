using Crypto.Models;
using Newtonsoft.Json;
using Serilog;
using System;
using System.IO;

namespace Crypto.DataHandling
{
    public class CryptoHelper
    {
        public static CoinmarketcapCredentials LoadCoinmarketcapCredentials(ILogger logger)
        {
            return LoadCredentials(logger)?.coinmarketcapCredentials;
        }

        public static RedditCredentials LoadRedditCredentials(ILogger logger)
        {
            return LoadCredentials(logger)?.redditCredentials;
        }

        private static Credentials LoadCredentials(ILogger logger)
        {
            try
            {
                var redditCredentials = File.ReadAllText((Path.Combine(Directory.GetCurrentDirectory(), "Credentials.json")));
                Credentials credentials = JsonConvert.DeserializeObject<Credentials>(redditCredentials);

                return credentials;
            }
            catch (FileNotFoundException e)
            {
                logger.Error("The file or directory cannot be found.");
                Console.WriteLine(e.Message);
                return null;

            }
            catch (UnauthorizedAccessException e)
            {
                logger.Error("You do not have permission to access this file.");
                Console.WriteLine(e.Message);
                return null;
            }
            catch (IOException e)
            {
                logger.Error("An unexpected error occured while loading the Reddit app credentials");
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
