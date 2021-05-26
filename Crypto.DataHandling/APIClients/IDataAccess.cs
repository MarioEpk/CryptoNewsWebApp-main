using Crypto.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.DataHandling.APIClients
{
    public interface IDataAccess
    {
        DataSource LoadData();
        Task SaveDataToDatabase(DataSource source);
        Task ClearOldEntries();

    }
}
