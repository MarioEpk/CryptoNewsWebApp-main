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
        private const int NUMBER_OF_POSTS = 10;

        public CryptoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Crypto
        public IActionResult Index()
        {
            var posts = _context.Post
                .OrderByDescending(post => post.CreatedAt)
                .Take(NUMBER_OF_POSTS)
                .Select(post => new PostViewModel
                {
                    Title = post.PostName,
                    PostedAt = post.PostedAt,
                    URL = post.PostURL
                }).ToList();

            return View(posts);
        }

        // GET: Search
        public IActionResult ShowSearchForm()
        {
            return View("ShowSearchForm");
        }

        // POST: Crypto/ShowSearchResults
        [HttpPost]
        public IActionResult ShowSearchResults(string searchPhrase)
        {
            var posts = _context.Post
                .Where(post => post.PostName.Contains(searchPhrase))
                .OrderByDescending(post => post.CreatedAt)
                .Select(post => new PostViewModel
                {
                    Title = post.PostName,
                    PostedAt = post.PostedAt,
                    URL = post.PostURL
                }).ToList();

            return View(posts);
        }
    }
}
