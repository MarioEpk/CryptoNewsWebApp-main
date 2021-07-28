using Crypto.Models;
using System.Threading.Tasks;

namespace Crypto.DataHandling.APIClients
{
    public interface IDataAccess
    {
        public DataSource LoadData();
        public Task SaveDataToDatabase(DataSource source);
        public Task ClearOldEntries();

    }
}
