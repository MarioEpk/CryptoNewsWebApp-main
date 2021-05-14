using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crypto.WebApplication.Models
{
    /// <summary>
    /// DB model for Reddit Posts
    /// </summary>
    public class Post
    {
        public int Id { get; set; }
        public string ServerID { get; set; }
        public string PostName { get; set; }
        public string PostURL { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
