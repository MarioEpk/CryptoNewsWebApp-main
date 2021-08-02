using System;
using System.Linq;
using Reddit;
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

        private const string NAME_OF_SUBREDDIT = "Cardano";
        private const int NUMBER_OF_POSTS = 10;

        public CardanoRedditClient(ApplicationDbContext context, ILogger logger)
        {
            _logger = logger;
            _context = context;
            try
            {
                RedditCredentials credentials = CryptoHelper.LoadRedditCredentials(_logger);
                if (credentials != null)
                {
                    _client = new RedditClient(appId: credentials.redditAppId, appSecret: credentials.redditAppSecret, refreshToken: credentials.redditRefreshToken);
                } else
                {
                    throw new Exception("Could not load credentials.");
                }
            }
            catch (Exception exception)
            {
                _logger.Error(exception.Message);
                throw exception;
            }
        }
        /// <summary>
        ///  Loads Data through the Reddit API
        /// </summary>
        /// <returns>DataSource object</returns>
        public async Task<DataSource> LoadData()
        {
            _logger.Debug("Fetching Data from the Reddit API...");

            var cryptoSubRedditPosts = _client.Subreddit(NAME_OF_SUBREDDIT).Posts.Hot;

            DataSource subreddit = new DataSource
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
                }).Take(NUMBER_OF_POSTS)
                .ToList(),

                Name = "CardanoReddit",
                CreatedAt = DateTime.Now,
                TypeOfSource = "Reddit"
            };

            _logger.Debug("Data fetched.");

            return await Task.FromResult(subreddit);
        }

        /// <summary>
        /// Saves the DataSource object into the database
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public async Task SaveDataToDatabase(DataSource source)
        {

            var existingPostIDs = _context.Post.Select(p => p.ServerID);

            source.Posts = source.Posts
                .Where(p => !existingPostIDs.Contains(p.ServerID))
                .ToList();

            await _context.AddAsync(source);
            await _context.SaveChangesAsync();

            _logger.Debug("Reddit data saved into the database.");
        }

        public async Task ClearOldEntries()
        {
            var oldDataSourceEntry = _context.DataSource
                .Where(datasource => datasource.CreatedAt < DateTime.Now.AddMonths(-3))
                .Include(d => d.Posts);

            var oldRedditPosts = oldDataSourceEntry
                .SelectMany(d => d.Posts);

            await oldRedditPosts.ForEachAsync(post =>
            {
               _context.Remove(post);
            });

            _logger.Debug("Old Reddit posts deleted from database");

            await oldDataSourceEntry.ForEachAsync(datasource =>
            {
                _context.Remove(datasource);
            });

            _logger.Debug("Old datasource entries deleted from database");

            await _context.SaveChangesAsync();
           
        }

    }
}
