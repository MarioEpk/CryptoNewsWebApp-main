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
        IDataAccess _dataAccess;      

        public BusinessLogic(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;      
        }

        public async Task ProcessData()
        {
            // fetching data
            var cardanoSubreddit = _dataAccess.LoadData();
            // Test save into db
            await _dataAccess.SaveDataToDatabase(cardanoSubreddit);

            
        }

        
    }
}
