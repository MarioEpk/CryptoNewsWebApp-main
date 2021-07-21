using Crypto.Models.Data;
using Crypto.WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Crypto.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var cardanoName = "Cardano";

            var latestCardanoCoin = _context.Coin
                .Where(coin => coin.Name.Equals(cardanoName))
                .OrderByDescending(coin => coin.CreatedAt)
                .FirstOrDefault();

            CoinViewModel cardano = new CoinViewModel
            {
                CMCRank = latestCardanoCoin.CMCRank,
                LatestPrice = latestCardanoCoin.Price,
                Name = latestCardanoCoin.Name
            };

            List<CoinViewModel> cardanoData = new List<CoinViewModel>();

            cardanoData.Add(new CoinViewModel
            {
                
            });

            return View(cardano);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
