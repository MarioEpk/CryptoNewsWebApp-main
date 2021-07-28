using System;
using System.Linq;
using Reddit;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Crypto.Models.Data;
using Crypto.Models;

namespace Crypto.DataHandling.APIClients
{
    public class CardanoRedditClient : IDataAccess
    {
        // DI
        private readonly RedditClient _client;
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        const string FILE_PATH = @"C:\Users\Epakf\source\repos\CryptoNewsWebApp-main\Crypto.DataHandling\RedditCredentials.txt";

        public string redditAppId;
        public string redditAppSecret;
        public string redditRefreshToken;

        public CardanoRedditClient(ApplicationDbContext context, ILogger logger)
        {
            _logger = logger;
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
            _logger.Debug("Fetching Data from the Reddit API...");

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
                    // CreatedAt points to the date the post was saved into DB
                    CreatedAt = DateTime.Now,
                    // PostedAt points to the date the original reddit post was created
                    PostedAt = s.Created
                }).Take(numberOfPosts).ToList(),

                Name = "CardanoReddit",
                CreatedAt = DateTime.Now,
                TypeOfSource = "Reddit"
            };

            _logger.Debug("Data fetched.");
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

            await _context.AddAsync(source);
            await _context.SaveChangesAsync();

            _logger.Debug("Reddit data saved into the database.");
        }

        // Loads reddit application credentials from file
        private void LoadCredentials()
        {
            try
            {
                var credentials = File.ReadAllLines(FILE_PATH);

                redditAppId = credentials[0];
                redditAppSecret = credentials[1];
                redditRefreshToken = credentials[2];
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);

            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task ClearOldEntries()
        {
            var oldRedditPosts = _context.Post.Where(post => post.CreatedAt < DateTime.Now.AddMonths(-1));

            var oldDataSourceEntry = _context.DataSource.Where(datasource => datasource.CreatedAt < DateTime.Now.AddMonths(-1));

            await oldDataSourceEntry.ForEachAsync(datasource =>
            {
                _context.Remove(datasource);
            });

            _logger.Debug("Old datasource entries deleted from database");

            await oldRedditPosts.ForEachAsync(post =>
             {
                 _context.Remove(post);
             });

            await _context.SaveChangesAsync();
            _logger.Debug("Old Reddit posts deleted from database");
        }

    }
}
