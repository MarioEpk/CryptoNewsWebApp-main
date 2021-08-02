using Crypto.DataHandling.APIClients;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crypto.DataHandling
{
    public class BusinessLogic : IBusinessLogic
    {
        private readonly List<IDataAccess> dataAccesses;

        public BusinessLogic(CardanoRedditClient redditClient, MarketCapClient marketCapClient)
        {
            dataAccesses = new List<IDataAccess>
            {
                redditClient,
                marketCapClient
            };
        }

        public async Task ProcessData()
        {
            foreach (IDataAccess dataAccess in dataAccesses)
            {
                var data = await dataAccess.LoadData();
                await dataAccess.SaveDataToDatabase(data);
                await dataAccess.ClearOldEntries();
            }
        }
    }
}
