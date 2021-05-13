using CryptoNewsWebApp.Data;
using CryptoNewsWebApp.Models;
using DataAPIRequests.APIClients;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAPIRequests
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
            List<IDataAccess> dataAccesses = new List<IDataAccess>();
            dataAccesses.Add(_redditClient);
            dataAccesses.Add(_marketCapClient);

            foreach(IDataAccess dataAccess in dataAccesses)
            {
                var data = dataAccess.LoadData();
                await dataAccess.SaveDataToDatabase(data);
            }
        }

        
    }
}
