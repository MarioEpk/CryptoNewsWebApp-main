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
    public class CardanoRedditClient : IDataAccess
    {
        RedditClient _client;
        ApplicationDbContext _context;

        const string FILE_PATH = @"C:\Users\Epakf\source\repos\CryptoNewsWebApp-main\DataAPIRequests\RedditCredentials.txt";

        public string redditAppId;
        public string redditAppSecret;
        public string redditRefreshToken;
            
        public CardanoRedditClient(ApplicationDbContext context)
        {
            _context = context;
            try
            {
                LoadCredentials();
                // Prerobit RedditClient na autofac
                
                _client = new RedditClient(appId: redditAppId, appSecret: redditAppSecret, refreshToken: redditRefreshToken);
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

            var cryptoSubRedditPosts = _client.Subreddit(nameOfSubreddit).Posts.Hot;

            subreddit = new DataSource
            {
                Posts = cryptoSubRedditPosts.Select(s => new Post
                {
                    PostName = s.Title,
                    PostURL = s.Listing.URL,
                    ServerID = s.Id,
                    CreatedAt = DateTime.Now

                }).Take<Post>(numberOfPosts).ToList(),

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
            //var postsAlreadyInDatabase = _context.Post.AsQueryable<Post>();

            var existingPostIDs = _context.Post.Select(p => p.ServerID);

            // filter the Posts in the DataSource source object, so only posts with unique ServerID are saved to the database    
            //source.Posts = source.Posts
            //    .Where(x => !postsAlreadyInDatabase.Any(y => y.ServerID.Equals(x.ServerID)))
            //    .ToList();

            source.Posts = source.Posts
                .Where(p => !existingPostIDs.Contains(p.ServerID))
                .ToList();

            await _context.AddAsync<DataSource>(source);
            await _context.SaveChangesAsync();

            Console.WriteLine("Reddit data saved into the database.");
        }

        // Loads reddit application credentials from file
        private void LoadCredentials()
        {
            try
            {
                var credentials = File.ReadAllLines(FILE_PATH);

                this.redditAppId = credentials[0];
                this.redditAppSecret = credentials[1];
                this.redditRefreshToken = credentials[2];
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
