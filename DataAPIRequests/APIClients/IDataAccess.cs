using CryptoNewsWebApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAPIRequests.APIClients
{
    public interface IDataAccess
    {
        DataSource LoadData();
        Task SaveDataToDatabase(DataSource source);

    }
}
