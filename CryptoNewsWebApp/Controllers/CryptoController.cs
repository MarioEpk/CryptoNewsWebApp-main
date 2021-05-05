using CryptoNewsWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoNewsWebApp.Data;
using Microsoft.EntityFrameworkCore;

namespace CryptoNewsWebApp.Controllers
{
    public class CryptoController : Controller
    {

        private readonly ApplicationDbContext _context;

        public CryptoController(ApplicationDbContext context)
        {
            this._context = context;
        }

        // GET: Crypto
        public async Task<IActionResult> Index()
        {
            // tu budem vyberat data z contextu DB a zobrazovat ich,index bude home, a spravim si este action na Reddit, na Coinmarket cap
            var numberOfPosts = 6;
            return View(await _context.Post.OrderByDescending(post => post.CreatedAt).Take(numberOfPosts).ToListAsync());
        }
        // GET: Search
        public async Task<IActionResult> ShowSearchForm()
        {
            return View("ShowSearchForm");
        }

        // POST: Crypto/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return View("index", await _context.Post.Where(j => j.PostName.Contains(SearchPhrase)).ToListAsync());
        }
    }
}
