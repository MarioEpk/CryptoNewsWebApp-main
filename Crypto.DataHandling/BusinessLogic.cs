using Crypto.DataHandling.APIClients;
using Crypto.WebApplication.Data;
using Crypto.WebApplication.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.DataHandling
{
    public class BusinessLogic : IBusinessLogic
    {
        private readonly CardanoRedditClient _redditClient;
        private readonly MarketCapClient _marketCapClient;

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
                await dataAccess.ClearOldEntries();
            }
        }


    }
}
