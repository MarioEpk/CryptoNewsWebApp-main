using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reddit;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CryptoNewsWebApp.Data;
using CryptoNewsWebApp.Models;

namespace DataAPIRequests.APIClients
{
    class CardanoRedditClient : IDataAccess
    {
        private RedditClient client;
        ApplicationDbContext _context;

        public string RedditAppId { get; set; }
        public string RedditAppSecret { get; set; }
        public string RedditRefreshToken { get; set; }

        public CardanoRedditClient(ApplicationDbContext context)
        {
            _context = context;
            try
            {
                LoadCredentials();
                // Prerobit RedditClient na autofac
                client = new RedditClient(appId: RedditAppId, appSecret: RedditAppSecret, refreshToken: RedditRefreshToken);
            }
            catch (TimeoutException exception)
            {
                throw exception;
            }
        }
        /// <summary>
        ///  Loads Data through the Reddit API
        /// </summary>
        /// <returns>DataSource object</returns>
        public DataSource LoadData()
        {
            Console.WriteLine("Fetching Data from Reddit API...");
            DataSource subreddit;
            string nameOfSubreddit = "Cardano";
            int numberOfPosts = 10;

            var cryptoSubRedditPosts = client.Subreddit(nameOfSubreddit).Posts.Hot;

            subreddit = new DataSource
            {
                Posts = cryptoSubRedditPosts.Select(s => new Post
                {
                    PostName = s.Title,
                    PostURL = s.Listing.URL,
                    ServerID = s.Id,
                    CreatedAt = DateTime.Now

                }).Take<Post>(numberOfPosts).ToList(),

                HomeURL = client.Subreddit(nameOfSubreddit).URL,
                Name = nameOfSubreddit,
                CreatedAt = DateTime.Now,
                TypeOfSource = "Reddit"
            };

            Console.WriteLine("Data fetched.");
            return subreddit;
        }
        /// <summary>
        /// Saves the DataSource object into the database
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public async Task SaveDataToDatabase(DataSource source)
        {
            var postsAlreadyInDatabase = _context.Post.AsQueryable<Post>();
            // filter the Posts in the DataSource source object, so only posts with unique ServerID are saved to the database    
            source.Posts = source.Posts.Where(x => !postsAlreadyInDatabase.Any(y => y.ServerID.Equals(x.ServerID))).ToList();

            await _context.AddAsync<DataSource>(source);
            await _context.SaveChangesAsync();

            Console.WriteLine("Data saved into the database.");
        }

        // Loads reddit application credentials from file
        private void LoadCredentials()
        {

            string FILE_PATH = @"C:\Users\Epakf\source\repos\CryptoNewsWebApp-main\DataAPIRequests\RedditCredentials.txt";

            try
            {
                var credentials = File.ReadAllLines(FILE_PATH);

                this.RedditAppId = credentials[0];
                this.RedditAppSecret = credentials[1];
                this.RedditRefreshToken = credentials[2];
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("The file or directory cannot be found.");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("You do not have permission to access this file.");
            }
            catch (IOException)
            {
                Console.WriteLine("An unexpected error occured while loading the Reddit app credentials");
            }
        }
    }
}
