using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Crypto.Models.Data;
using Crypto.WebApplication.Models;

namespace Crypto.WebApplication.Controllers
{
    public class CryptoController : Controller
    {
        private readonly ApplicationDbContext _context;
        List<PostViewModel> posts = new List<PostViewModel>();

        public CryptoController(ApplicationDbContext context)
        {
            this._context = context;
        }

        // GET: Crypto
        public async Task<IActionResult> Index()
        {
            var numberOfPosts = 10;

            var dbTopPosts = await _context.Post
                .OrderByDescending(post => post.CreatedAt)
                .Take(numberOfPosts)
                .ToListAsync();

            foreach(var post in dbTopPosts)
            {
                posts.Add(new PostViewModel
                {
                    Title = post.PostName,
                    PostedAt = post.PostedAt,
                    URL = post.PostURL
                });
            }

            return View(posts);
        }
        // GET: Search
        public async Task<IActionResult> ShowSearchForm()
        {
            return View("ShowSearchForm");
        }

        // POST: Crypto/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            var searchedPosts = await _context.Post
                .Where(j => j.PostName
                .Contains(SearchPhrase))
                .ToListAsync();

            foreach (var post in searchedPosts)
            {
                posts.Add(new PostViewModel
                {
                    Title = post.PostName,
                    PostedAt = post.PostedAt,
                    URL = post.PostURL
                });
            }

            return View(posts);
        }
    }
}
