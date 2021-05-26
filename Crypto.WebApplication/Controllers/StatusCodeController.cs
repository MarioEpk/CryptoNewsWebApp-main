using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crypto.WebApplication.Controllers
{
    public class StatusCodeController : Controller
    {
        public IActionResult Index(int code)
        {
            if (code == 404)
            {
                return View("404");
            }
            else if (code == 403)
            {
                return View("403");
            }
            else
            {
                return View("Generic", code);
            }
        }
    }
}
