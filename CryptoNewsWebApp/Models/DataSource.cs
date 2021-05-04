using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoNewsWebApp.Models
{
    public class DataSource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HomeURL { get; set; }
        public IEnumerable<Post> Posts { get; set; }
        public DateTime CreatedAt { get; set; }
        public string TypeOfSource { get; set; }
    }
}
