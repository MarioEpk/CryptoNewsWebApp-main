using Crypto.WebApplication.Data;
using Crypto.WebApplication.Models;
using DataHandling.APIClients;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataHandling
{
    public class BusinessLogic : IBusinessLogic
    {
        CardanoRedditClient _redditClient;
        MarketCapClient _marketCapClient;

        public BusinessLogic(CardanoRedditClient redditClient, MarketCapClient marketCapClient)
        {
            _redditClient = redditClient;
            _marketCapClient = marketCapClient;
        }

        public async Task ProcessData()
        {
            List<IDataAccess> dataAccesses = new List<IDataAccess>
            {
                _redditClient,
                _marketCapClient
            };

            //dataAccesses.ForEach(async dataAccess =>
            //{
            //    var data = dataAccess.LoadData();
            //    await dataAccess.SaveDataToDatabase(data);
            //});

            foreach (IDataAccess dataAccess in dataAccesses)
            {
                var data = dataAccess.LoadData();
                await dataAccess.SaveDataToDatabase(data);
            }
        }

        
    }
}
