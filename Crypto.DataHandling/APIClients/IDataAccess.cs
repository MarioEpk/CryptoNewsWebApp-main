using Crypto.WebApplication.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataHandling.APIClients
{
    public interface IDataAccess
    {
        DataSource LoadData();
        Task SaveDataToDatabase(DataSource source);

    }
}
